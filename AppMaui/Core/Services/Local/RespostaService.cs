using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;


namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar respostas de alunos aos desafios com validação de duplicidade.
    /// </summary>
    public class RespostaService(RespostaRepository _repostitorio, 
       EventoService eventoService)
    {
        /// <summary>
        /// Salva resposta apenas se o aluno não possui resposta prévia para o mesmo desafio.
        /// Registra este evento para notificar ao professor
        /// </summary>
        public async Task<bool> SalvarResposta(Resposta resposta)
        {
            try
            {
                if (resposta.AlunoId <= 0 || resposta.DesafioId <= 0)
                    return false;

                var respostaExistente = await _repostitorio.GetByAlunoDesafio(resposta.AlunoId, resposta.DesafioId);
                if (respostaExistente.Count ==0)
                {
                    resposta.UltimoRegistro = DateTime.Now;

                    await _repostitorio.Add(resposta);
                    EventoSistema eventoSistema = new()
                    {
                        Tabela = "Resposta",
                                      
                        Descricao = "Aluno salvou resposta.",
                        ReferenciaId = resposta.DesafioId,
                        UsuarioId = resposta.AlunoId                        
                    };
                    await eventoService.SalvarEvento(eventoSistema);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar resposta: {ex.Message}");
            }
        }

        public async Task<List<Resposta>> ListarRespostas()
        {
            try
            {
                var respostas = await _repostitorio.GetAll();
                return respostas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar respostas: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarResposta(Resposta resposta)
        {
            try
            {
                if (resposta.Id <= 0 ||
                    resposta.AlunoId <= 0 ||
                    resposta.DesafioId <= 0)
                    return false;

                resposta.UltimoRegistro = DateTime.Now;

                await _repostitorio.Update(resposta);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar resposta: {ex.Message}");
            }
        }

        public async Task<bool> DeletarResposta(int id)
        {
            try
            {
                if (id <= 0)
                    return false;
                var resposta = await _repostitorio.GetById(id);
                if (resposta == null)
                    return false;

                await _repostitorio.Delete(resposta);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar resposta: {ex.Message}");
            }
        }

        /// <summary>Retorna respostas de um aluno para todos os desafios de um livro específico.</summary>
        public async Task<List<Resposta>?> ListarAlunoDesafio(int livroId, int alunoId)
        {
            try
            {
                if (livroId <= 0 || alunoId <= 0)
                    throw new Exception("ID do livro ou aluno é inválido.");
                var respostas = await _repostitorio.BuscarRespostasLivro(alunoId, livroId);
                if (respostas == null)
                    return null;
                return respostas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar respostas por livro e usuário: {ex.Message}");
            }
        }
    }
}
