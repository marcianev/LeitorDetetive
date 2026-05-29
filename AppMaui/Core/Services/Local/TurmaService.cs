using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar turmas com integração de professor e trilha.
    /// </summary>
    public class TurmaService(TurmaRepository _repositorio, 
        ProfessorService _professorService)
    {
        public async Task<bool> SalvarTurma(Turma turma)
        {
            try
            {
                if (string.IsNullOrEmpty(turma.Nome) ||
                    turma.Nome.Length > 100 ||
                    turma.ProfessorId <= 0 ||
                    turma.TrilhaId <= 0)
                    return false;

                if (turma.TamanhoTrilha <= 0)
                    turma.TamanhoTrilha = 18;               
                turma.Status = true;
                turma.DataCriacao = DateTime.Now;
                turma.Ano = turma.DataCriacao.Year.ToString();

                await _repositorio.Add(turma);
                return true;
            }
            catch (Exception ex)
            {              
                throw new Exception($"Erro ao salvar turma: {ex.Message}");
            }
        }

        public async Task<List<Turma>?> ListarTurmaPorUsuario(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                    throw new Exception("id invalido");

                var professor = await _professorService.BuscarProfessorPorUsuario(idUsuario);
                if (professor == null)
                    return null;
                var turmas = await _repositorio.GetByProfessor(professor.Id);
                return turmas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar turmas: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarTurma(Turma turma)
        {
            try
            {
                if (turma.Id <= 0 || string.IsNullOrEmpty(turma.Nome) ||
                    turma.Nome.Length > 100 ||
                    turma.ProfessorId <= 0 ||
                    turma.TrilhaId <= 0 ||
                    turma.TamanhoTrilha <= 0 ||
                    turma.DataCriacao == DateTime.MinValue ||
                    turma.Status == null)
                {                   
                    return false;
                }     
                await _repositorio.Update(turma);
                return true;
            }
            catch (Exception ex)
            {               
                throw new Exception($"Erro ao atualizar turma: {ex.Message}");
            }
        }

        public async Task<bool> DeletarTurma(int id)
        {
            try
            {
                if (id <= 0)
                    return false;
                var turma = await _repositorio.GetById(id);
                if (turma == null)
                    return false;

                await _repositorio.Delete(turma);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar turma: {ex.Message}");
            }
        }

        public async Task<int> BuscarPorProfessor(int id)
        {
            try
            {
                if (id <= 0)
                    return 0;

                var turma = await _repositorio.GetOneByProfessor(id);
                if (turma == null)
                    return 0;
                return turma.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar turma: {ex.Message}");
            }
        }

        public async Task<Turma?> BuscarPorId(int id)
        {
            if (id <= 0)
                return null;

            return await _repositorio.GetById(id);
        }
    }
}
