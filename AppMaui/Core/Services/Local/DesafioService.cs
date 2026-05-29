using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Gerencia operações CRUD de desafios e população de dados iniciais.
    /// </summary>
    public class DesafioService(DesafioRepository _repository)
    {
        public async Task<bool> SalvarDesafio(Desafio desafio)
        {
            try
            {
                if (desafio.Pergunta == null || desafio.Resposta == null ||
                    desafio.Pergunta.Length > 100 ||
                    desafio.Resposta.Length > 20)
                    return false;

                await _repository.Add(desafio);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar desafio: {ex.Message}");
            }
        }

        public async Task<List<Desafio>> ListarDesafiosPorLivro(int livro)
        {
            try
            {
                if (livro <= 0)
                    throw new Exception("Livro inexistente.");

                return await _repository.GetByLivro(livro);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar desafios: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarDesafio(Desafio desafio)
        {
            try
            {
                if (desafio.Id <= 0 ||
                    string.IsNullOrEmpty(desafio.Resposta) || 
                    string.IsNullOrEmpty(desafio.Pergunta) ||
                    desafio.Pergunta.Length > 100 ||
                    desafio.Resposta.Length > 20)
                    return false;

                await _repository.Update(desafio);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar desafio: {ex.Message}");
            }
        }

        public async Task<bool> DeletarDesafio(int id)
        {
            try
            {
                if (id <= 0)
                    return false;

                var desafio = await _repository.GetById(id);
                if (desafio == null)
                    return false;

                await _repository.Delete(desafio);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar desafio: {ex.Message}");
            }
        }

        /// <summary>Popula tabela de desafios com dados do arquivo desafios.json se estiver vazia.</summary>
        public async Task PopularDesafios()
        {
            try
            {
                await _repository.PopularDesafios();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao popular desafios: {ex.Message}");
            }
        }
    }
}
