using ApiBackend.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;            
        }

        [HttpPost("teste")]
        public async Task<IActionResult> Teste()
        {
            bool enviado = await _emailService.EnviarEmail(
                "marcianev2@gmail.com",
                "TesteApi",
                "Email enviado pelo ApiBackend"
                );

            if (enviado)
                return Ok("Email enviado");
            return BadRequest("Falha ao enviar email");
        }
    }
}
