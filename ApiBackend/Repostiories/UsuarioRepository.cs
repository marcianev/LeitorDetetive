using ApiBackend.Data;
using ApiBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBackend.Repostiories
{
    public class UsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> GetByUser(string user)
        {
            return _context.Usuarios.FirstOrDefault(u => u.User == user);
        }

        public async Task<Usuario?> GetById(int id)
        {
            return _context.Usuarios.FirstOrDefault(u =>u.Id == id);
        }    
        
        public async Task<Usuario?> UpdateAsync(Usuario usuario)
        {
            _context.Update(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> UserExists(string user)
        {
            return await _context.Usuarios.AnyAsync(u => u.User == user);
            
        }
    }
}
