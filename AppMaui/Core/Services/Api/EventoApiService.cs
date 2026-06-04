using AppMaui.Core.Models;
using AppMaui.Core.Services.Api.Interface;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;
using System.Diagnostics;
using System.Net.Http.Json;


namespace AppMaui.Core.Services.Api
{
        public class EventoApiService : IEventoApiService
    {
        private readonly HttpClient _httpClient;

        public EventoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OperacaoResponse?> SalvarEvento(EventoSistema evento)
        {
            try
            {
                
                if(evento == null)
                {
                    return OperacaoResponse.Resposta(false, "Evento não pode ser nulo.");
                }                
                var registro = new EventoRequest
                {
                    Tabela = evento.Tabela,
                    TipoEvento = evento.TipoEvento,
                    Descricao = evento.Descricao,
                    DataEvento = evento.DataEvento,                   
                    UsuarioId = evento.UsuarioId
                };

                var response = await _httpClient.PostAsJsonAsync("api/evento/salvar-evento", registro);
               
                if (response == null)
                {
                    return OperacaoResponse.Resposta(false, "Resposta nula do servidor.");
                }
                return await response.Content.ReadFromJsonAsync<OperacaoResponse>();
            }
            catch (Exception ex)
            {
                return OperacaoResponse.Resposta(false, $"Exceção ao salvar evento: {ex.Message}");
            }
        }
    }
}
