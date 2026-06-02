using Shared.DTOs.Requests;
using Shared.DTOs.Requests.Auth;
using Shared.DTOs.Responses;
using Shared.DTOs.Responses.Auth;

namespace ApiBackend.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResponse?> Login(LoginRequest request);
        Task<bool> RecuperarSenha(RecuperarSenhaRequest request);
        Task<OperacaoResponse> AcessoProvisorio(AcessoProvisorioRequest request);
    }
}
