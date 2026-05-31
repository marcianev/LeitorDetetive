using ApiBackend.Data;
using ApiBackend.Models;

namespace ApiBackend.Repostiories
{
    public class ProfessorRepository
    {
        readonly AppDbContext _context;

        public ProfessorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Professor?> BuscarPorEmail(string email)
        {
            return _context.Professores.FirstOrDefault(p => p.Email == email);
        }

        public async Task<Professor?> BuscarPorUsuarioId(int UsuarioId)
        {
            return _context.Professores.FirstOrDefault(p => p.UsuarioId == UsuarioId);
        }
    }
}
