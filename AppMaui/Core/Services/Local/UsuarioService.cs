using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _repositorio;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _repositorio = usuarioRepository;
        }

        //metodo salvar
        public async Task<Usuario?> SalvarUsuario(Usuario usuario)
        {
            try
            {
                //validações
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
        }//fim salvar

        //metodo listar
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
        }//fim listar

        //metodo atualizar
        public async Task<bool> AtualizarUsuario(Usuario usuario)
        {
            try
            {
                //validações
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
                throw new Exception ($"Erro ao atualizar usuário: {ex.Message}");
            }
        }//fim atualizar

        //metodo deletar
        public async Task<bool> DeletarUsuario(int id)
        {
            try
            {
                //validação
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
               throw new Exception ($"Erro ao deletar usuário: {ex.Message}");
            }
        }//fim deletar

        //buscar id de usuario por nome
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
        }//fim buscar id de usuario por nome           

        //buscar usuario por id
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
        }//fim buscar usuario por id
    }
}
