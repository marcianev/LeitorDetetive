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

        public async Task<Usuario?> GetByUser(string user)
        {
            return _context.Usuarios.FirstOrDefault(u => u.User == user);
        }

        public async Task<Usuario?> GetById(int id)
        {
            return _context.Usuarios.FirstOrDefault(u =>u.Id == id);
        }

        public async Task Update(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            
        }

    }
}
