using Shared.DTOs.Requests;
using Shared.DTOs.Responses;

namespace ApiBackend.Services.Interface
{
    public interface ICadastroService
    {
        Task<OperacaoResponse> CadastrarProfessor(CadastroProfessorRequest request);
    }
}
