using AppMaui.Core.Services.Api.Interface;
using AppMaui.Core.Services.Local;
using Shared.DTOs.Requests;
using Shared.DTOs.Requests.Auth;
using Shared.DTOs.Responses;
using Shared.DTOs.Responses.Auth;
using System.Diagnostics;
using System.Net.Http.Json;

namespace AppMaui.Core.Services.Api
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;        

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                    "api/auth/login",
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

        public async Task<bool> RecuperarSenha(RecuperarSenhaRequest request)
        {
            try
            {         
                var response = await _httpClient.PostAsJsonAsync(
                    "api/auth/recuperar-senha",
                    request);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<OperacaoResponse?> AcessoProvisorio(AcessoProvisorioRequest request)
        {
                var response = await _httpClient.PostAsJsonAsync(
                    "api/auth/acesso-provisorio",
                    request);

                return await response.Content.ReadFromJsonAsync<OperacaoResponse>();          
        }
    }
}
