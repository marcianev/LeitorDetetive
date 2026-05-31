using ApiBackend.Data;
using ApiBackend.Models;

namespace ApiBackend.Repostiories
{
    public class UsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> BuscarPorUser(string user)
        {
            return _context.Usuarios.FirstOrDefault(u => u.User == user);
        }

        public async Task<Usuario?> BuscarPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(u =>u.Id == id);
        }
    }
}
