using AppMaui.Core.Services.Api.Interface;
using Shared.Requests.Auth;
using Shared.Responses.Auth;
using System.Diagnostics;
using System.Net.Http.Json;

namespace AppMaui.Core.Services.Api
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<LoginResponse?> Login(
            string user,
            string senha)
        {
            try
            {
                LoginRequest request = new()
                {
                    User = user,
                    Senha = senha
                };
                var response = await _httpClient.PostAsJsonAsync(
                    "https://localhost:7170/api/auth/login",
                    request);

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<LoginResponse?>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
            
        }
    }
}
