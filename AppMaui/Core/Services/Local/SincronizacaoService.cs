using AppMaui.Core.Models.Sincronizacao;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;
using AppMaui.Core.Services.Api.Interface;
using AppMaui.Core.Services.Local.Interfaces;
using AppMaui.Services;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;
using System.Text.Json;

namespace AppMaui.Core.Services.Local
{
    public class SincronizacaoService(ConectividadeService conectividadeService,
        EventoService eventoService, IEventoApiService eventoApiService,
        ITurmaApiService turmaApiService, TurmaRepository turmaRepository) : ISincronizacaoService
    {

        private readonly string _caminhoExclusoes = Path.Combine(FileSystem.AppDataDirectory, "exclusao.json");


        public async Task<OperacaoResponse> SincronizarTudo()
        {
            if (!conectividadeService.TemInternet())
                return new OperacaoResponse { Sucesso = false, Mensagem = "Sem conexão com a internet." };

            var eventos = await SincronizarEventos();
            if (!eventos.Sucesso)
                return new OperacaoResponse { Sucesso = false, Mensagem = $"Falha ao sincronizar eventos: {eventos.Mensagem}" };

            var turmas = await SincronizarTurmas();
            if(!turmas.Sucesso)
                return new OperacaoResponse { Sucesso = false, Mensagem = $"Falha ao sincronizar turmas: {turmas.Mensagem}" };

            return turmas;
        }

        public async Task<OperacaoResponse> SincronizarEventos()
        {
            var eventosNaoSincronizados = await eventoService.ListarEventosNaoSincronizados();
            if (eventosNaoSincronizados.Count == 0)
            {
                return new OperacaoResponse { Sucesso = true, Mensagem = "Nenhum evento para sincronizar." };
            }

            foreach (var sincronizar in eventosNaoSincronizados)
            {
                var eventoApi = await eventoApiService.SalvarEvento(sincronizar);
                if (eventoApi?.Sucesso == true)
                {
                    sincronizar.Sincronizado = true;
                    sincronizar.DataSincronizado = DateTime.Now;
                    bool atualizado = await eventoService.AtualizarEvento(sincronizar);
                    if (!atualizado)
                        return new OperacaoResponse { Sucesso = false, Mensagem = $"Evento sincronizado, mas falha ao atualizar status local." };
                }
            }
            return new OperacaoResponse { Sucesso = true, Mensagem = "Todos os eventos sincronizados com sucesso." };
        }

        public async Task<OperacaoResponse> SincronizarTurmas()
        {
            try
            {
                if (!conectividadeService.TemInternet())
                {
                    return new OperacaoResponse
                    {
                        Sucesso = false,
                        Mensagem = "Sem conexão."
                    };
                }                

                var exclusoes = await CarregarExclusoes();

                var exclusoesProcessadas = new List<ExclusaoPendente>();

                foreach (var exclusao in exclusoes
                    .Where(x => x.Tabela == "Turma"))
                {
                    var resultado =
                        await turmaApiService.Delete(
                            exclusao.Uuid.ToString());

                    if (resultado.Sucesso)
                    {
                        exclusoesProcessadas.Add(exclusao);
                    }
                }

                foreach (var exclusao in exclusoesProcessadas)
                {
                    exclusoes.Remove(exclusao);
                }

                if (exclusoesProcessadas.Count > 0)
                {
                    await AtualizarExclusoes(exclusoes);
                }
               

                var turmasPendentes =
                    await turmaRepository.GetNaoSincronizadas();

                foreach (var turma in turmasPendentes)
                {
                    TurmaRequest request = new()
                    {
                        Uuid = turma.Uuid,
                        Nome = turma.Nome,
                        Ano = turma.Ano,
                        CodigoAcesso = turma.CodigoAcesso,
                        DataCriacao = turma.DataCriacao,
                        DataAlterado = turma.DataAlterado,
                        ProfessorId = turma.ProfessorId,
                        Status = turma.Status,
                        TamanhoTrilha = turma.TamanhoTrilha,
                        TrilhaId = turma.TrilhaId
                    };

                    var resultado =
                        await turmaApiService.Add(request);

                    if (!resultado.Sucesso)
                        continue;

                    turma.Sincronizado = true;
                    turma.DataSincronizado = DateTime.Now;

                    await turmaRepository.Update(turma);
                }

                return new OperacaoResponse
                {
                    Sucesso = true,
                    Mensagem = "Sincronização concluída."
                };
            }
            catch (Exception ex)
            {
                return new OperacaoResponse
                {
                    Sucesso = false,
                    Mensagem = $"Erro na sincronização: {ex.Message}"
                };
            }
        }



        private async Task<List<ExclusaoPendente>> CarregarExclusoes()
        {
            try
            {
                if (!File.Exists(_caminhoExclusoes))
                {
                    await File.WriteAllTextAsync(
                        _caminhoExclusoes,
                        "[]");
                }

                    string json = await File.ReadAllTextAsync(_caminhoExclusoes);

                    if (string.IsNullOrWhiteSpace(json))
                        return [];

                    return JsonSerializer.Deserialize<List<ExclusaoPendente>>(json) ?? [];                
            }
            catch
            {
                return [];
            }
        }

        public async Task SalvarExclusoes(ExclusaoPendente exclusaoPendente)
        {
            var exclusoes = await CarregarExclusoes();

            exclusoes.Add(exclusaoPendente);

            string json = JsonSerializer.Serialize(exclusaoPendente,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
            await File.WriteAllTextAsync(_caminhoExclusoes, json);
        }

        private async Task AtualizarExclusoes(List<ExclusaoPendente> listaExclusaoPendentes)
        {
            string json = JsonSerializer.Serialize(listaExclusaoPendentes,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            await File.WriteAllTextAsync(_caminhoExclusoes, json);
        }
    }
}
