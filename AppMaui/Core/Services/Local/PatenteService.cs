using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar patentes com validações e população de dados iniciais.
    /// </summary>
    public class PatenteService(PatenteRepository _repositorio)
    {
        public async Task<bool> SalvarPatente(Patente patente)
        {
            try
            {
                if (string.IsNullOrEmpty(patente.Nome) ||
                    patente.Nome.Length > 100)
                    return false;

                await _repositorio.Add(patente);
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar o aluno: {ex.Message}");
            }
        }

        public async Task<List<Patente>> ListarPatentesPorTrilha(int id)
        {
            try
            {
                if (id == 0)
                    throw new Exception("Trilha inválida.");
                var patentes = await _repositorio.GetByTrilha(id);
                return patentes;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao listar patentes: {e.Message}");
            }
        }

        public async Task<bool> AtualizarPatente(Patente patente)
        {
            try
            {
                if (string.IsNullOrEmpty(patente.Nome) ||
                    patente.Nome.Length > 100 ||
                    patente.Nivel <= 0 ||
                    patente.Nivel < 0 ||
                    patente.TrilhaId == 0)
                    return false;                

                await _repositorio.Update(patente);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao atualizar a patente: {e.Message}");
            }
        }

        public async Task<bool> DeletarPatente(int id)
        {
            try
            {
                if (id <= 0)
                    return false;
                var patente = await _repositorio.GetById(id);
                if(patente == null)
                    return false;

                await _repositorio.Delete(patente);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao deletar patente: {e.Message}");
            }
        }

        /// <summary>Popula a tabela de patentes com dados iniciais do arquivo JSON.</summary>
        public async Task PopularPatentes()
        {
            try
            {
                await _repositorio.PopularPatentes();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao popular livros: {ex.Message}");
            }
        }
    }
}
