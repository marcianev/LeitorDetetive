using ApiBackend.Repostiories;
using ApiBackend.Services.Interface;
using Shared.Requests.Auth;
using Shared.Responses.Auth;

namespace ApiBackend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IJwtService _jwtService;

        public AuthService(IUsuarioService usuarioService, IJwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse?> Login(LoginRequest request)
        {
            var usuario = await _usuarioService.BuscarPorUser(request.User);

            if (usuario == null)
                return null;

            if (request.Senha != request.Senha)
                return null;

            return new LoginResponse
            {
                UsuarioId = usuario.Id,
                User = usuario.User,
                Tipo = usuario.Tipo,
                Token = _jwtService.GerarToken(usuario),
                StatusUsuario = usuario.StatusUsuario,
                StatusSenha = usuario.StatusSenha
            };
        }
    }
}
