using ApiBackend.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests;

namespace ApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private readonly ICadastroService _cadastroService;

        public CadastroController(ICadastroService cadastroService)
        {
            _cadastroService = cadastroService;
        }

        [HttpPost("professor")]
        public async Task<IActionResult> CadastrarProfessor(CadastroProfessorRequest request)
        {
            var response = await _cadastroService.CadastrarProfessor(request);

            if (!response.Sucesso) 
                return BadRequest(response);

            return Ok(response);
        }
    }
}
