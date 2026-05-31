using ApiBackend.Repostiories;
using Shared.Requests.Auth;
using Shared.Responses.Auth;

namespace ApiBackend.Services
{
    public class AuthService
    {
        private readonly UsuarioRepository _usuariosRepository;
        private readonly JwtService _jwtService;

        public AuthService(UsuarioRepository usuariosRepository, JwtService jwtService)
        {
            _usuariosRepository = usuariosRepository;
            _jwtService = jwtService;
        }

        public LoginResponse? Login(LoginRequest request)
        {
            var usuario = _usuariosRepository.BuscarPorUser(request.User);

            if (usuario == null)
                return null;

            if (request.Senha != request.Senha)
                return null;

            return new LoginResponse
            {
                UsuarioId = usuario.Id,
                User = usuario.User,
                Tipo = usuario.Tipo,
                Token = _jwtService.GerarToken(usuario)
            };
        }
    }
}
