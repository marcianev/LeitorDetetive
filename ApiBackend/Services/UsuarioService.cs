using ApiBackend.Models;
using ApiBackend.Repostiories;
using ApiBackend.Services.Interface;

namespace ApiBackend.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> AddAsync(Usuario usuario)
        {
            return await _usuarioRepository.AddAsync(usuario);
        }

        public async Task<Usuario?> GetByIUser(string user)
        {
            return await _usuarioRepository.GetByUser(user);
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _usuarioRepository.GetById(id);
        }

        public async Task<Usuario?> UpdateAsync(Usuario usuario)
        {
            return await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task<bool> UserExists(string user)
        {
            return await _usuarioRepository.UserExists(user);
        }
    }
}
