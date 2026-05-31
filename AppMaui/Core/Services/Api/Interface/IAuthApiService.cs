using Shared.Responses.Auth;

namespace AppMaui.Core.Services.Api.Interface
{
    public interface IAuthApiService
    {
        Task<LoginResponse?> Login(
            string user,
            string senha);
    }
}
