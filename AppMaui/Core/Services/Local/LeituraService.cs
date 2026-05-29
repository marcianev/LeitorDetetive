using AppMaui.Core.DTOs;
using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar leituras de alunos, rastreamento de progresso e análises.
    /// </summary>
    public class LeituraService(LeituraRepository _repositorio, AlunoService _aluno, ProfessorService _professorService)
    {      
        public async Task<bool> SalvarLeitura(Leitura leitura)
        {
            try
            {
                if (leitura.UsuarioId <= 0 ||leitura.LivroId <= 0)
                    return false;

                leitura.DataInicio = DateTime.Now;
                leitura.Status = StatusLeitura.Iniciada;

                await _repositorio.Add(leitura);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar leitura: {ex.Message}");
            }
        }

        public async Task<List<Leitura>> ListarLeituras(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new Exception("ID do usuário é inválido.");

                var leituras = await _repositorio.GetByUsuario(usuarioId);
                return leituras;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar leituras: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarLeitura(Leitura leitura)
        {
            try
            {
                if (leitura.Id <= 0 ||
                    leitura.UsuarioId <= 0 ||
                    leitura.DataInicio == default ||
                    leitura.LivroId <= 0)
                    return false;
                string s = leitura.Status.ToString();
                if (s.Length > 20)
                    return false;

                await _repositorio.Update(leitura);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar leitura: {ex.Message}");
            }
        }

        public async Task<bool> DeletarLeitura(int leituraId)
        {
            try
            {
                if (leituraId <= 0)
                    return false;
                var leitura = await _repositorio.GetById(leituraId);
                if (leitura == null)
                    return false;

                await _repositorio.Delete(leitura);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar leitura: {ex.Message}");
            }
        }

        public async Task<Leitura?> GetLeituraPorLivroUsuario(int livroId, int usuarioId)
        {
            try
            {
                if (livroId <= 0 || usuarioId <= 0)
                    throw new Exception("IDs de livro e usuário são inválidos.");
                var leitura = await _repositorio.GetByLivroUsuario(livroId, usuarioId);
                return leitura;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar leitura: {ex.Message}");
            }
        }

        public async Task<Leitura?> GetLeituraAtualPorUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new Exception("ID do usuário é inválido.");
                var leitura = await _repositorio.GetLeituraAtualPorUsuario(usuarioId);
                return leitura;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar leitura atual: {ex.Message}");
            }
        }

        public async Task<Leitura?> GetLeituraAnteriorPorUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    throw new Exception("ID do usuário é inválido.");
                var leitura = await _repositorio.GetLeituraAnteriorPorUsuario(usuarioId);
                return leitura;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar leitura anterior: {ex.Message}");
            }
        }

        /// <summary>Marca a leitura atual como concluída, atualiza data de término e verifica progressão de patente.</summary>
        public async Task<bool> ConcluirLeitura(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    return false;
                var leitura = await _repositorio.GetLeituraAtualPorUsuario(usuarioId);
                if (leitura == null)
                    return false;
                leitura.DataFim = DateTime.Now;
                leitura.Status = StatusLeitura.Concluida;
                await _repositorio.Update(leitura);
                int leituras = await _repositorio.ContarLeiturasConcluidas(usuarioId);
                return await _aluno.AtualizarPatente(usuarioId, leituras);             
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao concluir leitura: {ex.Message}");
            }
        }

        /// <summary>Retorna os 5 livros mais lidos das turmas do professor.</summary>
        public async Task<List<LivrosMaisLidosDTO>?> RankingLeitura(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    return null;

                var professor = await _professorService.BuscarProfessorPorUsuario(usuarioId);
                if (professor == null)
                    return null;               

                return await _repositorio.GetTopLivrosProfessor(professor.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar maiores leituras: {ex.Message}");
            }
        }
    }
}
