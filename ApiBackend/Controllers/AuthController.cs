using ApiBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Requests.Auth;

namespace ApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var resultado = _authService.Login(request);

            if (resultado == null) 
                return Unauthorized("Usuário ou senha inválidos.");

            return Ok(resultado);
        }
    }
}
