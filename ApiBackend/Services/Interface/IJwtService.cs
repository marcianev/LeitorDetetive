using ApiBackend.Models;

namespace ApiBackend.Services.Interface
{
    public interface IJwtService
    {
        string GerarToken(Usuario usuario);
    }
}
