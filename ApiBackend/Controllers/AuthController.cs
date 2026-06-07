using ApiBackend.Services;
using ApiBackend.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests;
using Shared.DTOs.Requests.Auth;
using Shared.DTOs.Responses;
using System.Diagnostics;

namespace ApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;      

        public AuthController(IAuthService authService)
        {
            _authService = authService;         
        }

        [HttpGet("teste")]
        public IActionResult Teste()
        {
            return Ok("API funcionando");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {           
            var resultado = await _authService.Login(request);

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

        [HttpPost("acesso-provisorio")]
        public async Task<IActionResult> AcessoProvisorio(AcessoProvisorioRequest request)
        {
            OperacaoResponse response = await _authService.AcessoProvisorio(request);

            if (!response.Sucesso)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
