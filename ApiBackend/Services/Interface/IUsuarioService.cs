using ApiBackend.Models;

namespace ApiBackend.Services.Interface
{
    public interface IUsuarioService
    {
        Task<Usuario> AddAsync(Usuario usuario);
        Task<Usuario?> GetByIUser(string user);
        Task<Usuario?> GetById(int id);
        Task<Usuario?> UpdateAsync(Usuario usuario);
        Task<bool> UserExists(string user);
    }
}
