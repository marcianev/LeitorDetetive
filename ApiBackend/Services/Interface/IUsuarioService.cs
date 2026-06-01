using ApiBackend.Models;

namespace ApiBackend.Services.Interface
{
    public interface IUsuarioService
    {
        Task<Usuario?> BuscarPorUser(string user);
        Task<Usuario?> BuscarPorId(int id);
        Task Atualizar(Usuario usuario);
    }
}
