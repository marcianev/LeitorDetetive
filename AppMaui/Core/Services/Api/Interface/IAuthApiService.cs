using Shared.DTOs.Requests;
using Shared.DTOs.Responses.Auth;

namespace AppMaui.Core.Services.Api.Interface
{
    public interface IAuthApiService
    {
        Task<LoginResponse?> Login(string user, string senha);
        Task<bool> RecuperarSenha(RecuperarSenhaRequest request);
    }
}
