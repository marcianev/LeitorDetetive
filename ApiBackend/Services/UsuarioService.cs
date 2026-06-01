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

        public async Task<Usuario?> BuscarPorUser(string user)
        {
            return await _usuarioRepository.GetByUser(user);
        }

        public async Task<Usuario?> BuscarPorId(int id)
        {
            return await _usuarioRepository.GetById(id);
        }

        public async Task Atualizar(Usuario usuario)
        {
            await _usuarioRepository.Update(usuario);
        }
    }
}
