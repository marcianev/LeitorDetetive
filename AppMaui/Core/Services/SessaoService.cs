using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Services.Local;
using Shared.DTOs.Responses.Auth;
using Shared.Enums;

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
        }

        /// <summary>Encerra a sessão do usuário logado.</summary>
        public static void Logout()
        {
            UsuarioLogado = null;
            Token = null;
            LoginRemoto=false;
        }
    }
}
