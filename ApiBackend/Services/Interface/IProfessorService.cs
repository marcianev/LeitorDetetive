using ApiBackend.Models;

namespace ApiBackend.Services.Interface
{
    public interface IProfessorService
    {
        Task<Professor> AddAsync(Professor professor);
        Task<Professor?> GetByEmail(string email);
        Task<Professor?> GetByUsuarioId(int usuarioId);
        Task<bool> EmailExists(string email);
        Task<bool> CpfExists(string cpf);
    }
}
