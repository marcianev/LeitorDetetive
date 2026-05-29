using AppMaui.Core.Models;
using AppMaui.Core.Services.Local;
using AppMaui.Services.Interfaces;

namespace AppMaui.Core.Services.Application
{
    /// <summary>
    /// Serviço responsável pela autenticação de usuários com validação de credenciais e hash.
    /// </summary>
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly UsuarioService _usuarioService;
        private readonly ICriptoService _criptoService;
        private readonly SessaoService _sessaoService;

        public AutenticacaoService(
            UsuarioService usuarioService,
            ICriptoService criptoService,
            SessaoService sessaoService)
        {
            _usuarioService = usuarioService;
            _criptoService = criptoService;
            _sessaoService = sessaoService;
        }

        /// <summary>Autentica usuário validando credenciais e registrando sessão ativa.</summary>
        public async Task<Usuario?> Autenticar(string user, string senha)
        {
            var usuario = await _usuarioService.BuscarUsuarioPorUser(user);

            if (usuario == null)
                return null;

            if (!_criptoService.VerificarHash(senha, usuario.Senha))
                return null;

            await _sessaoService.Login(usuario);

            return usuario;
        }
    }
}
