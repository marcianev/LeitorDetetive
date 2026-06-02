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

        public async Task<Professor> AddAsync(Professor professor)
        {
            return await _professorRepository.AddAsync(professor);
        }

        public async Task<Professor?> GetByEmail(string email)
        {
            return await _professorRepository.GetByEmail(email);
        }

        public async Task<Professor?> GetByUsuarioId(int usuarioId)
        {
            return await _professorRepository.GetByUsuarioId(usuarioId);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _professorRepository.EmailExists(email);
        }

        public async Task<bool> CpfExists(string cpf)
        {
            return await _professorRepository.CpfExists(cpf);
        }
    }
}
