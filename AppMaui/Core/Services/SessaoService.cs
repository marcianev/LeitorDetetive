using AppMaui.Core.Models;
using AppMaui.Core.Services.Local;
using Shared.DTOs.Responses.Auth;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace AppMaui.Core.Services
{
    /// <summary>
    /// Serviço estático para gerenciamento de sessão do usuário logado na aplicação.
    /// </summary>
    public class SessaoService(EventoService eventoService)
    {
        /// <summary>Armazena o usuário atualmente autenticado na sessão.</summary>
        public static Usuario? UsuarioLogado { get; private set; }

        public static string? Token { get; private set; }
        
        public static bool LoginRemoto { get; private set; }

        /// <summary>Autentica um usuário e abre sua sessão.</summary>
        public async Task Login(Usuario usuario)
        {
            UsuarioLogado = usuario;

            EventoSistema eventoSistema = new()
            {
                Tabela = "Usuario",
               
                Descricao = "Usuario acessou o sistema",
                ReferenciaId = usuario.Id,
                UsuarioId = usuario.Id
            };

            await eventoService.SalvarEvento(eventoSistema);


        }

        ///<summary>Criar sessao remota</summary>
        public async Task LoginApi(LoginResponse response)
        {
            UsuarioLogado = new Usuario
            {
                Id = response.UsuarioId,
                User = response.User,
                Tipo = response.Tipo,
                StatusSenha = response.StatusSenha,
                StatusUsuario = response.StatusUsuario

            };
            Token = response.Token;
            LoginRemoto = true;    
            
            string json = JsonSerializer.Serialize(response);
            Debug.WriteLine(json);
            await SecureStorage.SetAsync("sessaoUsuario", json);
        }

        //<sumary>Verifica se há uma sessão ativa e carrega os dados do usuário logado.</summary>
        public async Task<bool> RestaurarSessao()
        {
            try
            {
                string? json = await SecureStorage.GetAsync("sessaoUsuario");

                if (string.IsNullOrWhiteSpace(json))
                    return false;

                var response = JsonSerializer.Deserialize<LoginResponse>(json);

                if (response == null)
                    return false;

                UsuarioLogado = new Usuario
                {
                    Id = response.UsuarioId,
                    User = response.User,
                    Tipo = response.Tipo,
                    StatusSenha = response.StatusSenha,
                    StatusUsuario = response.StatusUsuario
                };

                Token = response.Token;
                LoginRemoto = true;

                return true;
            }
            catch
            {
                return false;
            }
        }

        ///<summary>Validar Token.</summary>
        public async Task<bool> ValidarToken()
        {
            string? json = await SecureStorage.GetAsync("sessaoUsuario");

            if (string.IsNullOrWhiteSpace(json))
                return false;

            var response = JsonSerializer.Deserialize<LoginResponse>(json);
            
            if(response == null || string.IsNullOrWhiteSpace(response.Token))
                return false;

            JwtSecurityTokenHandler handler = new();
            
            JwtSecurityToken jwt = handler.ReadJwtToken(response.Token);

            DateTime expiracao = jwt.ValidTo;

            return expiracao > DateTime.UtcNow;
        }

        /// <summary>Encerra a sessão do usuário logado.</summary>
        public static void Logout()
        {
            UsuarioLogado = null;
            Token = null;
            LoginRemoto=false;
            SecureStorage.Remove("sessao");
        }
    }
}
