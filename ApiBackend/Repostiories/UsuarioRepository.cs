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

        public Usuario? BuscarPorUser(string user)
        {
            return _context.Usuarios.FirstOrDefault(u => u.User == user);
        }
    }
}
