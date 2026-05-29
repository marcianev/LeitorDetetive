using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar usuários com validações e persistência.
    /// </summary>
    public class UsuarioService
    {
        private readonly UsuarioRepository _repositorio;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _repositorio = usuarioRepository;
        }

        public async Task<Usuario?> SalvarUsuario(Usuario usuario)
        {
            try
            {
                if (string.IsNullOrEmpty(usuario.User) ||
                    await _repositorio.GetByUser(usuario.User) != null ||
                    usuario.User.Length > 50 ||
                    string.IsNullOrEmpty(usuario.Tipo))
                    return null;

                Usuario u = new Usuario
                {
                    User = usuario.User,
                    Tipo = usuario.Tipo,
                    StatusSenha = usuario.StatusSenha,
                    Senha = usuario.Senha,
                    StatusUsuario = usuario.StatusUsuario,
                    DataCadastro = DateTime.Now
                };

                await _repositorio.Add(u);
                return u;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro salvar aluno: {ex.Message}");
            }
        }

        public async Task<List<Usuario>> ListarUsuarios()
        {
            try
            {
                var usuarios = await _repositorio.GetAll();
                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar usuários: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarUsuario(Usuario usuario)
        {
            try
            {
                if (usuario.Id <= 0 ||
                    await _repositorio.GetById(usuario.Id) == null ||
                    string.IsNullOrEmpty(usuario.User) ||
                    usuario.User.Length > 50 ||
                    string.IsNullOrEmpty(usuario.Senha) ||
                    usuario.StatusSenha == null || 
                    usuario.StatusUsuario == null ||
                    string.IsNullOrEmpty(usuario.Tipo))
                    return false;               

                await _repositorio.Update(usuario);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar usuário: {ex.Message}");
            }
        }

        public async Task<bool> DeletarUsuario(int id)
        {
            try
            {
                if (id <= 0)
                    return false;
                var usuario = await _repositorio.GetById(id);

                if (usuario == null)
                    return false;

                await _repositorio.Delete(usuario);
                return true;
            }
            catch (Exception ex)
            {
               throw new Exception($"Erro ao deletar usuário: {ex.Message}");
            }
        }

        public async Task<Usuario?> BuscarUsuarioPorUser(string user)
        {
            try
            {
                if (string.IsNullOrEmpty(user))
                    return null;

                var usuario = await _repositorio.GetByUser(user);
                if (usuario == null)
                    return null;

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário por User {ex.Message}");
            }
        }

        public async Task<Usuario?> BuscarUsuarioPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return null;
                var usuario = await _repositorio.GetById(id);
                if (usuario == null)
                    return null;

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário por ID: {ex.Message}");
            }
        }
    }
}
