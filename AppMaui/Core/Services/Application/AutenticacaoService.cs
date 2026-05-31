using AppMaui.Core.Models;
using AppMaui.Core.Services.Api.Interface;
using AppMaui.Core.Services.Local;
using AppMaui.Services.Interfaces;
using System.Diagnostics;

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
        private readonly IAuthApiService _authApiService;

        public AutenticacaoService(
            UsuarioService usuarioService,
            ICriptoService criptoService,
            SessaoService sessaoService,
            IAuthApiService authApiService)
        {
            _usuarioService = usuarioService;
            _criptoService = criptoService;
            _sessaoService = sessaoService;
            _authApiService = authApiService;
        }

        /// <summary>Autentica usuário validando credenciais e registrando sessão ativa.</summary>
        public async Task<Usuario?> Autenticar(string user, string senha)
        {
            var loginApi = await _authApiService.Login(user, senha);
           
            if (loginApi != null)
            {
                await _sessaoService.LoginApi(loginApi);                
                return SessaoService.UsuarioLogado;
            }
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
