using ApiBackend.Models;

namespace ApiBackend.Services.Interface
{
    public interface IProfessorService
    {
        Task<Professor?> BuscarPorEmail(string email);
        Task<Professor?> BuscarPorUsuarioId(int usuarioId);
    }
}
