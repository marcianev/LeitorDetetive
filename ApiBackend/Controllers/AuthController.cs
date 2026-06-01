using ApiBackend.Services;
using ApiBackend.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests;
using Shared.DTOs.Requests.Auth;
using System.Diagnostics;

namespace ApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {           
            var resultado = _authService.Login(request);

            if (resultado == null)
                return Unauthorized("Usuário ou senha inválidos.");

            return Ok(resultado);
        }

        [HttpPost("recuperar-senha")]
        public async Task<IActionResult> RecuperarSenha(RecuperarSenhaRequest request)
        {
            bool resultado = await _authService.RecuperarSenha(request);          

            if (!resultado)
                return BadRequest("Email não encontrado");

            return Ok("Código Enviado.");
        }
    }
}
