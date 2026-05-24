using AppMaui.Core.Enums;
using AppMaui.Core.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.External
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;       
        private readonly OpenAISettings _openAISettings;

        public OpenAIService(OpenAISettings openAISettings)
        {
            _httpClient = new HttpClient();
            _openAISettings = openAISettings;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _openAISettings.ApiKey);
        }

        public async Task<StatusAvaliacao> ValidarComentario(string comentario)
        {
            var corpo = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = $"Você é um moderador de comentários de leitura infantil." +
                        $"Analise o comentário abaixo: {comentario}. Aprove comentários normais " +
                        $"de opinião, mesmo que simples, curtos ou com pequenos erros de português." +
                        $"Reprove apenas comentários que contenham:\r\n- palavrões\r\n- ofensas\r" +
                        $"\n- discurso de ódio\r\n- conteúdo sexual\r\n- spam\r\n- texto sem sentido\r" +
                        $"\n- conteúdo totalmente fora do contexto do livro\r\n\r" +
                        $"\nResponda APENAS com:\r" +
                        $"\n\r\nAPROVADO\r" +
                        $"\nou\r\nREPROVADO"                         
                    }
                }
            };

            var json = JsonSerializer.Serialize(corpo);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var resposta = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                content
            );
           
            if (!resposta.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao conectar a api.");            
            }

            var respostaJson = await resposta.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(respostaJson);

            var texto =
                doc.RootElement
                   .GetProperty("choices")[0]
                   .GetProperty("message")
                   .GetProperty("content")
                   .GetString();
           
            return texto switch
            {                
                "APROVADO" => StatusAvaliacao.Aprovada,
                "REPROVADO" => StatusAvaliacao.AnaliseManual,
                _ => StatusAvaliacao.AnaliseAutomatica
            };
        }
    }
}
