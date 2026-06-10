using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Models.Sincronizacao;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;
using AppMaui.Core.Services.Local.Interfaces;
using Shared.DTOs.Responses;
using System.Text.Json;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Serviço para gerenciar turmas com integração de professor e trilha.
    /// </summary>
    public class TurmaService(TurmaRepository _repositorio, 
        ProfessorService _professorService, ISincronizacaoService _sincronizacaoService)
    {
        private readonly string _caminhoExclusoes = Path.Combine(FileSystem.AppDataDirectory, "exclusao.json");

        public async Task<OperacaoResponse<Turma>> SalvarTurma(Turma turma)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(turma.Nome) ||
                    turma.Nome.Length > 100 ||
                    turma.ProfessorId <= 0 ||
                    turma.TrilhaId <= 0)
                {
                    return new OperacaoResponse<Turma>
                    {
                        Sucesso = false,
                        Mensagem = "Dados inválidos para salvar a turma."
                    };
                }

                turma.Uuid = Guid.NewGuid();

                if (turma.TamanhoTrilha <= 0)
                    turma.TamanhoTrilha = 18;

                turma.Status = true;
                turma.DataCriacao = DateTime.Now;
                turma.DataAlterado = DateTime.Now;

                turma.Sincronizado = false;
                turma.DataSincronizado = null;

                turma.Ano = turma.DataCriacao.Year.ToString();

                await _repositorio.Add(turma);               

                var resultado = await _sincronizacaoService.SincronizarTurmas();

                if (resultado.Sucesso)
                {                  
                    turma.Sincronizado = true;
                    turma.DataSincronizado = DateTime.Now;
                    await _repositorio.Update(turma);
                }

                return new OperacaoResponse<Turma>
                {
                    Sucesso = true,
                    Mensagem = "Turma salva com sucesso.",
                    Dados = turma
                };
            }
            catch (Exception ex)
            {
                return new OperacaoResponse<Turma>
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao salvar turma: {ex.Message}"
                };
            }
        }

        public async Task<OperacaoResponse<List<Turma>>> ListarTurmaPorUsuario(int idUsuario)
        {
            try
            {
                //await _sincronizacaoService.SincronizarTurmas();

                if (idUsuario <= 0)
                {
                    return new OperacaoResponse<List<Turma>>
                    {
                        Sucesso = false,
                        Mensagem = "Usuário inválido."
                    };
                }

                var professor =
                    await _professorService.BuscarProfessorPorUsuario(idUsuario);

                if (professor == null)
                {
                    return new OperacaoResponse<List<Turma>>
                    {
                        Sucesso = false,
                        Mensagem = "Professor não encontrado."
                    };
                }

                var turmas =
                    await _repositorio.GetByProfessor(professor.Id);

                return new OperacaoResponse<List<Turma>>
                {
                    Sucesso = true,
                    Mensagem = "Turmas encontradas.",
                    Dados = turmas ?? []
                };
            }
            catch (Exception ex)
            {
                return new OperacaoResponse<List<Turma>>
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar turmas: {ex.Message}"
                };
            }
        }

        public async Task<OperacaoResponse> AtualizarTurma(Turma turma)
        {
            try
            {
                if (turma.Id <= 0 ||
                    string.IsNullOrWhiteSpace(turma.Nome) ||
                    turma.ProfessorId <= 0 ||
                    turma.TrilhaId <= 0)
                {
                    return OperacaoResponse.Resposta(
                        false,
                        "Dados inválidos para atualização.");
                }

                turma.DataAlterado = DateTime.Now;
                turma.Sincronizado = false;
                turma.DataSincronizado = null;

                await _repositorio.Update(turma);

                var resposta = await _sincronizacaoService.SincronizarTurmas();
                if (resposta.Sucesso)
                {
                    turma.Sincronizado = true;
                    turma.DataSincronizado = DateTime.Now;
                    await _repositorio.Update(turma);
                }

                return OperacaoResponse.Resposta(
                    true,
                    "Turma atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                return OperacaoResponse.Resposta(
                    false,
                    $"Erro ao atualizar turma: {ex.Message}");
            }
        }

        public async Task<OperacaoResponse> DeletarTurma(int id)
        {
            try
            {
                if (id <= 0)
                    return OperacaoResponse.Resposta(false, "Id inválido.");

                var turma = await _repositorio.GetById(id);

                if (turma == null)
                    return OperacaoResponse.Resposta(false, "Turma não encontrada.");              

                var deletado = await _repositorio.Delete(turma);

                if (deletado <= 0)
                    return new OperacaoResponse
                    {
                        Sucesso = false,
                        Mensagem = "Erro ao deletar turma."
                    };

                ExclusaoPendente exclusao = new()
                {
                    Tabela = "Turma",
                    Uuid = turma.Uuid,
                    DataExclusao = DateTime.Now
                };
                await _sincronizacaoService.SalvarExclusoes(exclusao);

                var resposta = await _sincronizacaoService.SincronizarTurmas();
                if (resposta.Sucesso)
                {
                    turma.Sincronizado = true;
                    turma.DataSincronizado = DateTime.Now;
                    await _repositorio.Update(turma);
                }      
                return OperacaoResponse.Resposta(
                    true,
                    "Turma excluída com sucesso.");
            }
            catch (Exception ex)
            {
                return OperacaoResponse.Resposta(
                    false,
                    $"Erro ao excluir turma: {ex.Message}");
            }
        }

        public async Task<OperacaoResponse<int?>> BuscarPorProfessor(int professorId)
        {
            try
            {    
                var turma =
                    await _repositorio.GetOneByProfessor(professorId);

                return new OperacaoResponse<int?>
                {
                    Sucesso = turma != null,
                    Mensagem = turma != null
                        ? "Turma encontrada."
                        : "Turma não encontrada.",
                    Dados = turma?.Id 
                };
            }
            catch (Exception ex)
            {
                return new OperacaoResponse<int?>
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                };
            }
        }

        public async Task<OperacaoResponse<Turma?>> BuscarPorId(int id)
        {
            try
            {
               // await _sincronizacaoService.SincronizarTurmas();

                var turma = await _repositorio.GetById(id);

                return new OperacaoResponse<Turma?>
                {
                    Sucesso = turma != null,
                    Mensagem = turma != null
                        ? "Turma encontrada."
                        : "Turma não encontrada.",
                    Dados = turma
                };
            }
            catch (Exception ex)
            {
                return new OperacaoResponse<Turma?>
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                };
            }
        }

        public async Task<OperacaoResponse<List<Turma?>>> GetNãoSincronizadas()
        {
            try
            {
                var turmas = await _repositorio.GetNaoSincronizadas();

                return new OperacaoResponse<List<Turma?>>
                {
                    Sucesso = true,
                    Mensagem = "Turmas não sincronizadas encontradas.",
                    Dados = turmas ?? []
                };

            }
            catch (Exception ex)
            {
                return new OperacaoResponse<List<Turma?>>
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao buscar turmas não sincronizadas: {ex.Message}"
                };
            }
        }       
    }
}
