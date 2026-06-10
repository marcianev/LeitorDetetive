using ApiBackend.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests;

namespace ApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController(IEventoService eventoService) : ControllerBase
    {   
        [HttpPost("salvar-evento")]
        public async Task<IActionResult> SalvarEvento(EventoRequest request)
        {
            var resultado = await eventoService.SalvarEvento(request);
            if (!resultado.Sucesso)
                return BadRequest(resultado);
            return Ok(resultado);
        }

    }
}
