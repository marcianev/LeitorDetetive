using AppMaui.Core.Models;
using AppMaui.Core.Services.Local;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;

namespace AppMaui.Core.Services.Api.Interface
{
    public interface ITurmaApiService
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
