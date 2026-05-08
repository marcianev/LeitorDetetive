using System;
using System.Collections.Generic;
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
        private const string API_KEY = "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";

        public OpenAIService()
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", API_KEY);
        }

        public async Task<string> ValidarComentario(string comentario)
        {
            var corpo = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = $"Analise o comentário de um aluno:\n\n\"{comentario}\"\n\n" +
                                    "Responda apenas com APROVADO ou REPROVADO. " +
                                    "Considere linguagem imprópria e relevância educacional."
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
                return $"Erro: {resposta.StatusCode}";
            }

            var respostaJson = await resposta.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(respostaJson);

            var texto =
                doc.RootElement
                   .GetProperty("choices")[0]
                   .GetProperty("message")
                   .GetProperty("content")
                   .GetString();

            return texto ?? "Sem resposta";
        }
    }
}
