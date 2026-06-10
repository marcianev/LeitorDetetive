using ApiBackend.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Requests;

namespace ApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurmaController(ITurmaService turmaService) : ControllerBase
    {
        [HttpGet("teste")]
        public IActionResult Teste()
        {
            return Ok("Controller funcionando");
        }

        [HttpPost("salvar-turma")]
        public async Task<IActionResult> AddTurma(TurmaRequest request)
        {
            var resultado = await turmaService.Add(request);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetTurmas()
        {
            var resultado = await turmaService.GetAll();

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpPut("update-turma")]
        public async Task<IActionResult> UpdateTurma(TurmaRequest request)
        {
            var resultado = await turmaService.Update(request);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpDelete("{uuid}")]
        public async Task<IActionResult> DeleteTurma(string uuid)
        {
            var resultado = await turmaService.Delete(uuid);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("professor/{professorId}")]
        public async Task<IActionResult> GetTurmasByProfessor(int professorId)
        {
            var resultado = await turmaService.GetByProfessor(professorId);

            if (!resultado.Sucesso)
                return BadRequest(resultado);
            return Ok(resultado);
        }

        [HttpGet("{uuid}")]
        public async Task<IActionResult> GetByUuid(string uuid)
        {
            var resultado = await turmaService.GetByUuid(uuid);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }

        [HttpGet("um-professor/{professorId}")]
        public async Task<IActionResult> GetOneByProfessor(int professorId)
        {
            var resultado = await turmaService.GetOneByProfessor(professorId);

            if (!resultado.Sucesso)
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
}
