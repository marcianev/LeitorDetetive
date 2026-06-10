using ApiBackend.Models;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;

namespace ApiBackend.Services.Interface
{
    public interface ITurmaService
    {
        Task<OperacaoResponse<TurmaResponse?>> Add(TurmaRequest request);

        Task<OperacaoResponse<List<TurmaResponse>>> GetAll();

        Task<OperacaoResponse<List<TurmaResponse>>> GetByProfessor(int professorId);

        Task<OperacaoResponse<TurmaResponse?>> GetByUuid(string uuid);

        Task<OperacaoResponse> Update(TurmaRequest request);

        Task<OperacaoResponse> Delete(string uuid);

        Task<OperacaoResponse<TurmaResponse?>> GetOneByProfessor(int professorId);
    }
}
