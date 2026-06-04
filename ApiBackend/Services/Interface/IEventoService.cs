using Shared.DTOs.Requests;
using Shared.DTOs.Responses;

namespace ApiBackend.Services.Interface
{
    public interface IEventoService
    {
        Task<OperacaoResponse> SalvarEvento(EventoRequest request);
    }
}
