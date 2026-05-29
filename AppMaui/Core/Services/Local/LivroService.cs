using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;


namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar livros com validações e população de dados iniciais.
    /// </summary>
    public class LivroService(LivroRepository _repositorio)
    {
        public async Task<bool> SalvarLivro(Livro livro)
        {
            try
            {
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
                throw new Exception($"Erro ao salvar livro: {ex.Message}");
            }
        }

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
        }

        public async Task<bool> AtualizarLivro(Livro livro)
        {
            try
            {
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
                throw new Exception($"Erro ao atualizar livro: {ex.Message}");
            }
        }

        public async Task<bool> DeletarLivro(int id)
        {
            try
            {
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
                throw new Exception($"Erro ao deletar livro: {ex.Message}");
            }
        }

        public async Task<Livro?> BuscarLivroPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return null;
                var livro = await _repositorio.GetById(id);
                return livro;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar livro por id: {ex.Message}");
            }
        }

        /// <summary>Popula a tabela de livros com dados iniciais do arquivo JSON.</summary>
        public async Task PopularLivros()
        {
            try
            {
                await _repositorio.PopularLivros();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao popular livros: {ex.Message}");
            }
        }
    }
}
