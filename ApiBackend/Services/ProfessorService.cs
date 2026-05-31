using ApiBackend.Models;
using ApiBackend.Repostiories;
using ApiBackend.Services.Interface;

namespace ApiBackend.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly ProfessorRepository _professorRepository;

        public ProfessorService(ProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public async Task<Professor?> BuscarPorEmail(string email)
        {
            return await _professorRepository.BuscarPorEmail(email);
        }

        public async Task<Professor?> BuscarPorUsuarioId(int usuarioId)
        {
            return await _professorRepository.BuscarPorUsuarioId(usuarioId);
        }
    }
}
