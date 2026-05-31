using Shared.Responses.Auth;
using Shared.Requests.Auth;

namespace ApiBackend.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResponse?> Login(LoginRequest request);
    }
}
