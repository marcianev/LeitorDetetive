
using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;
using AppMaui.Core.Services.External;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Gerencia avaliações de livros com validação automática de comentários via OpenAI.
    /// </summary>
    public class AvaliacaoService(AvaliacaoRepository _repositorio, 
        OpenAIService openAIService, ProfessorService _professorService)   
    {          
        /// <summary>Salva avaliação após validação automática de conteúdo via OpenAI.</summary>
        public async Task<bool> SalvarAvaliacao(AvaliacaoDTO dto)
        {
            try
            {
                if (dto.Nota < 1 || dto.Nota > 5 ||
                    string.IsNullOrEmpty(dto.Comentario) ||
                    dto.Comentario.Length > 300)
                    return false;

                var status = await openAIService.ValidarComentario(dto.Comentario);

                var avaliacao = new Avaliacao()
                {
                    Nota = dto.Nota,
                    Comentario = dto.Comentario,
                    Status = status,
                    UsuarioId = dto.UsuarioId,
                    LivroId = dto.LivroId,
                    DataCadastro = DateTime.Now
                };

                await Task.Delay(2000);
                var salvo = await _repositorio.Add(avaliacao);
                return salvo > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar avaliação: {ex.Message}");
            }
        }

        /// <summary>Lista todas as avaliações de alunos pertencentes a uma turma.</summary>
        public async Task<List<Avaliacao>> ListarAvaliacoes(int turmaId)
        {
            try
            {
                if (turmaId <= 0)
                    throw new Exception("ID da turma é inválido.");

                return await _repositorio.GetByTurma(turmaId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar avaliações: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarAvaliacao(AvaliacaoDTO dto)
        {
            try
            {
                if (dto.IdAvaliacao <= 0 || dto.Nota < 1 || dto.Nota > 5 ||
                    string.IsNullOrEmpty(dto.Comentario) ||
                    dto.Comentario.Length > 100)
                    return false;

                var avaliacao = new Avaliacao
                {
                    Id = dto.IdAvaliacao,
                    Nota = dto.Nota,
                    Comentario = dto.Comentario,
                    DataCadastro = dto.DataCadastro,
                    LivroId = dto.LivroId,
                    Status = dto.Status,
                    UsuarioId = dto.UsuarioId
                };

                await _repositorio.Update(avaliacao);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar avaliação: {ex.Message}");
            }
        }

        public async Task<bool> DeletarAvaliacao(int id)
        {
            try
            {
                if (id <= 0)
                    return false;

                var avaliacao = await _repositorio.GetById(id);
                if (avaliacao == null)
                    return false;

                await _repositorio.Delete(avaliacao);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar avaliação: {ex.Message}");
            }
        }

        /// <summary>Retorna todas as avaliações de um livro com dados do aluno e turma.</summary>
        public async Task<List<AvaliacaoDTO>?> ListarPorLivro(int livroId)
        {
            try
            {
                if (livroId <= 0)
                    return null;

                return await _repositorio.GetByLivro(livroId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar comentários: {ex.Message}");
            }
        }

        /// <summary>Retorna avaliações de um livro apenas das turmas do professor.</summary>
        public async Task<List<AvaliacaoDTO>?> ListarPorLivroTurma(int idLivro, int idUsuario)
        {
            try
            {
                if (idLivro <= 0 || idUsuario <= 0)
                    return null;

                var professor = await _professorService.BuscarProfessorPorUsuario(idUsuario);
                if(professor == null)
                    return null;
                return await _repositorio.GetByLivroProfessor(idLivro, professor.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar livros: {ex.Message}");
            }
        }

        /// <summary>Retorna os 5 livros mais bem avaliados pelas turmas do professor.</summary>
        public async Task<List<LivrosBemAvaliadosDTO>?> TopAvaliados(int idUsuario)
        {
            if (idUsuario <= 0)
                return null;

            var professor = await _professorService.BuscarProfessorPorUsuario(idUsuario);
            if (professor == null)
                return null;

            return await _repositorio.GetBemAvalidado(professor.Id); 
        }
    }
}
