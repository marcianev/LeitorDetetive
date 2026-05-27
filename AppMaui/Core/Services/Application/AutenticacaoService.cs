using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using AppMaui.Core.Services.Local;
using AppMaui.Core.Services.Security;
using AppMaui.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Application
{
    /// <summary>
    /// Serviço responsável pela autenticação de usuários com validação de credenciais e hash.
    /// </summary>
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly UsuarioService _usuarioService;
        private readonly ICriptoService _criptoService;

        public AutenticacaoService(
            UsuarioService usuarioService,
            ICriptoService criptoService)
        {
            _usuarioService = usuarioService;
            _criptoService = criptoService;
        }

        /// <summary>Autentica usuário validando credenciais e registrando sessão ativa.</summary>
        public async Task<Usuario?> Autenticar(string user, string senha)
        {
            var usuario = await _usuarioService.BuscarUsuarioPorUser(user);

            if (usuario == null)
                return null;

            if (!_criptoService.VerificarHash(senha, usuario.Senha))
                return null;

            SessaoService.Login(usuario);

            return usuario;
        }
    }
}
