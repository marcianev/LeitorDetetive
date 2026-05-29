using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;


namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar trilhas de aprendizado com validações.
    /// </summary>
    public class TrilhaService(TrilhaRepository _repositorio)
    {
        public async Task<bool> SalvarTrilha(Trilha turma)
        {
            try
            {
                if (string.IsNullOrEmpty(turma.Nome) ||
                    turma.Nome.Length > 100)
                    return false;

                await _repositorio.Add(turma);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar trilha: {ex.Message}");
            }
        }

        public async Task<List<Trilha>> ListarTrilha()
        {
            try
            {
                var trilhas = await _repositorio.GetAll();
                return trilhas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar trilhas: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarTrilha(Trilha turma)
        {
            try
            {
                if (turma.Id <= 0 ||
                    string.IsNullOrEmpty(turma.Nome) ||
                    turma.Nome.Length > 100)
                    return false;

                await _repositorio.Update(turma);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar trilha: {ex.Message}");
            }
        }

        public async Task<bool> DeletarTrilha(int id)
        {
            try
            {
                if (id <= 0)
                    return false;
                var trilha = await _repositorio.GetById(id);
                if(trilha == null)
                    return false;

                await _repositorio.Delete(trilha);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar trilha: {ex.Message}");
            }
        }
    }
}
