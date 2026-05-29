using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Services.Logging;

namespace AppMaui.Core.Services
{
    /// <summary>
    /// Serviço estático para gerenciamento de sessão do usuário logado na aplicação.
    /// </summary>
    public class SessaoService(EventoService eventoService)
    {
        /// <summary>Armazena o usuário atualmente autenticado na sessão.</summary>
        public static Usuario? UsuarioLogado { get; private set; }

        /// <summary>Autentica um usuário e abre sua sessão.</summary>
        public async Task Login(Usuario usuario)
        {
            UsuarioLogado = usuario;

            EventoSistema eventoSistema = new()
            {
                Tabela = "Usuario",
                TipoEvento = Eventos.Login,
                Descricao = "Usuario acessou o sistema",
                ReferenciaId = usuario.Id,
                UsuarioId = usuario.Id
            };

            await eventoService.SalvarEvento(eventoSistema);


        }

        /// <summary>Encerra a sessão do usuário logado.</summary>
        public static void Logout()
        {
            UsuarioLogado = null;
        }
    }
}
