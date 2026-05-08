using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local
{
    public class LivroService(LivroRepository _repositorio)
    {
        //mettodo salva livro
        public async Task<bool> SalvarLivro(Livro livro)
        {
            try
            {
                //validações
                if (string.IsNullOrEmpty(livro.Titulo) ||
                    livro.Titulo.Length > 100 ||
                    string.IsNullOrEmpty(livro.Autor) ||
                    livro.Autor.Length > 100 ||
                    (livro.Ilustrador != null && livro.Ilustrador.Length > 100) ||
                    livro.Editora != null && livro.Editora.Length > 50)
                    return false;

                await _repositorio.Add(livro);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao salvar livro: {ex.Message}");
            }
        }//fecha método salvar livro

        //metodo para listar livros
        public async Task<List<Livro>> ListarLivros()
        {
            try
            {
                var livros = await _repositorio.GetAll();
                return livros;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar livros: {ex.Message}");
            }
        }//fim listar

        //metodo para atualizar livro
        public async Task<bool> AtualizarLivro(Livro livro)
        {
            try
            {
                //validações
                if (livro.Id <= 0 ||
                    string.IsNullOrEmpty(livro.Titulo) ||
                    livro.Titulo.Length > 100 ||
                    string.IsNullOrEmpty(livro.Autor) ||
                    livro.Autor.Length > 100 ||
                    livro.Ilustrador != null && livro.Ilustrador.Length > 100 ||
                    livro.Editora != null && livro.Editora.Length > 50)
                    return false;

                await _repositorio.Add(livro);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao atualizar livro: {ex.Message}");
            }
        }//fim atualizar

        //metodo para deletar livro
        public async Task<bool> DeletarLivro(int id)
        {
            try
            {
                //validação
                if (id <= 0)
                    return false;
                var livro = await _repositorio.GetById(id);
                if (livro == null)
                    return false;

                await _repositorio.Delete(livro);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception ($"Erro ao deletar livro: {ex.Message}");
            }
        }//fim deletar
    }
}
