using ApiBackend.Models;
using ApiBackend.Repostiories;
using ApiBackend.Services.Interface;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;

namespace ApiBackend.Services
{
    public class TurmaService(TurmaRepository repository) : ITurmaService    {
      

        public async Task<OperacaoResponse<TurmaResponse?>> Add(TurmaRequest request)
        {
            try
            {
                if (request == null)
                    return new OperacaoResponse<TurmaResponse?> { Sucesso = false, Mensagem = "Request is null" };

                var turma = new Turma
                {
                    Uuid = request.Uuid,
                    Nome = request.Nome,
                    DataCriacao = request.DataCriacao,
                    Ano = request.Ano,
                    TamanhoTrilha = request.TamanhoTrilha,
                    Status = request.Status,
                    CodigoAcesso = request.CodigoAcesso,
                    ProfessorId = request.ProfessorId,
                    TrilhaId = request.TrilhaId,
                    DataAlterado = request.DataAlterado
                };
                Console.WriteLine("Antes do repository.Add");

                var adicionada = await repository.Add(turma);

                Console.WriteLine($"Turma retornada: {adicionada != null}");

                if (adicionada == null)
                    return new OperacaoResponse<TurmaResponse?> { Sucesso= false, Mensagem = "Erro ao criar turma." };

                TurmaResponse response = new TurmaResponse
                {
                    Uuid = adicionada.Uuid,
                    Nome = adicionada.Nome,
                    DataCriacao = adicionada.DataCriacao,
                    Ano = adicionada.Ano,
                    TamanhoTrilha = adicionada.TamanhoTrilha,
                    Status = adicionada.Status,
                    CodigoAcesso = adicionada.CodigoAcesso,
                    ProfessorId = adicionada.ProfessorId,
                    TrilhaId = adicionada.TrilhaId,
                    DataAlterado = adicionada.DataAlterado
                };

                return new OperacaoResponse<TurmaResponse?> { Sucesso= true, Mensagem = "Turma adicionada com sucesso", Dados = response};
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO: {ex}");

                if (ex.InnerException != null)
                    Console.WriteLine($"INNER: {ex.InnerException}");

                return new OperacaoResponse<TurmaResponse?>
                {
                    Sucesso = false,
                    Mensagem = ex.Message
                };               
            }
        }

        public async Task<OperacaoResponse<List<TurmaResponse>>> GetAll()
        {
            try
            {
                var turmas = await repository.GetAll();                

                var responseList = turmas.Select(turma => new TurmaResponse
                {
                    Uuid = turma.Uuid,
                    Nome = turma.Nome,
                    DataCriacao = turma.DataCriacao,
                    Ano = turma.Ano,
                    TamanhoTrilha = turma.TamanhoTrilha,
                    Status = turma.Status,
                    CodigoAcesso = turma.CodigoAcesso,
                    ProfessorId = turma.ProfessorId,
                    TrilhaId = turma.TrilhaId,
                    DataAlterado = turma.DataAlterado
                }).ToList();

                return new OperacaoResponse<List<TurmaResponse>> { Sucesso= true, Mensagem = "Turmas encontradas.", Dados = responseList };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving turmas: {ex.Message}");
                return new OperacaoResponse<List<TurmaResponse>> { Sucesso= false, Mensagem = "Erro ao recuperar turmas" };
            }
        }

        public async Task<OperacaoResponse<List<TurmaResponse>>> GetByProfessor(int professorId)
        {
            try
            {
                if(professorId == 0)
                    return new OperacaoResponse<List<TurmaResponse>> { Sucesso= false, Mensagem = "Invalid professor ID" };

                var turmas = await repository.GetByProfessor(professorId);
                
                var responseList = turmas.Select(turma => new TurmaResponse
                {
                    Uuid = turma.Uuid,
                    Nome = turma.Nome,
                    DataCriacao = turma.DataCriacao,
                    Ano = turma.Ano,
                    TamanhoTrilha = turma.TamanhoTrilha,
                    Status = turma.Status,
                    CodigoAcesso = turma.CodigoAcesso,
                    ProfessorId = turma.ProfessorId,
                    TrilhaId = turma.TrilhaId,
                    DataAlterado = turma.DataAlterado
                }).ToList();

                return new OperacaoResponse<List<TurmaResponse>> { Sucesso= true, Mensagem = "Turmas encontradas.", Dados = responseList };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving turmas by professor: {ex.Message}");
                return new OperacaoResponse<List<TurmaResponse>> { Sucesso= false, Mensagem = "Erro ao recuperar turmas por professor" };
            }
        }
       
        public async Task<OperacaoResponse<TurmaResponse?>> GetByUuid(string uuid)
        {
            try
            {
                if (string.IsNullOrEmpty(uuid))
                    return new OperacaoResponse<TurmaResponse?> { Sucesso = false, Mensagem = "UUID da turma inválido." };

                var turma = await repository.GetByUuid(uuid);
                if (turma == null)
                    return new OperacaoResponse<TurmaResponse?> { Sucesso = false, Mensagem = "Turma não encontrada" };

                var response = new TurmaResponse
                {
                    Uuid = turma.Uuid,
                    Nome = turma.Nome,
                    DataCriacao = turma.DataCriacao,
                    Ano = turma.Ano,
                    TamanhoTrilha = turma.TamanhoTrilha,
                    Status = turma.Status,
                    CodigoAcesso = turma.CodigoAcesso,
                    ProfessorId = turma.ProfessorId,
                    TrilhaId = turma.TrilhaId,
                    DataAlterado = turma.DataAlterado
                };

                return new OperacaoResponse<TurmaResponse?> { Sucesso= true, Mensagem = "Turma recuperada com sucesso", Dados = response };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving turma by UUID: {ex.Message}");
                return new OperacaoResponse<TurmaResponse?> { Sucesso= false, Mensagem = "Erro ao recuperar turma por ID" };
            }
        }

        public async Task<OperacaoResponse> Update(TurmaRequest request)
        {
            try
            {
                if(request == null)
                    return new OperacaoResponse { Sucesso = false, Mensagem = "Turma é nula." };

                var turma = new Turma
                {
                    Uuid = request.Uuid,
                    Nome = request.Nome,
                    TamanhoTrilha = request.TamanhoTrilha,
                    Status = request.Status,
                    ProfessorId = request.ProfessorId,
                    TrilhaId = request.TrilhaId,
                    DataAlterado = request.DataAlterado
                };

                var updated = await repository.Update(turma);
                if (!updated)
                    return new OperacaoResponse { Sucesso = false, Mensagem = "Erro ao atualizar turma." };

                return new OperacaoResponse<bool> { Sucesso= updated, Mensagem = "Turma atualizada com sucesso" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating turma: {ex.Message}");
                return new OperacaoResponse<bool> { Sucesso= false, Mensagem = "Erro ao atualizar turma" };
            }
        }

        public async Task<OperacaoResponse> Delete(string uuid)
        {
            try
            {
                if (string.IsNullOrEmpty(uuid))
                    return new OperacaoResponse { Sucesso = false, Mensagem = "UUID da turma inválido." };
                  
                var deleted = await repository.Delete(uuid);
                if (!deleted)
                    return new OperacaoResponse { Sucesso = false, Mensagem = "Erro ao excluir turma." };

                return new OperacaoResponse { Sucesso= true, Mensagem = "Turma excluída com sucesso"};
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar turma: {ex.Message}");
                return new OperacaoResponse { Sucesso= false, Mensagem = "Erro ao excluir turma" };
            }
        }

        public async Task<OperacaoResponse<TurmaResponse?>> GetOneByProfessor(int id)
        {
            try
            {
                if (id == 0)
                    return new OperacaoResponse<TurmaResponse?> { Sucesso = false, Mensagem = "Id professor inválido." };


                var turma = await repository.GetOneByProfessor(id);
                if (turma == null)
                    return new OperacaoResponse<TurmaResponse?> { Sucesso = false, Mensagem = "Turma não encontrada" };

                var response = new TurmaResponse
                {
                    Uuid = turma.Uuid,
                    Nome = turma.Nome,
                    DataCriacao = turma.DataCriacao,
                    Ano = turma.Ano,
                    TamanhoTrilha = turma.TamanhoTrilha,
                    Status = turma.Status,
                    CodigoAcesso = turma.CodigoAcesso,
                    ProfessorId = turma.ProfessorId,
                    TrilhaId = turma.TrilhaId,
                    DataAlterado = turma.DataAlterado
                };

                return new OperacaoResponse<TurmaResponse?> { Sucesso = true, Mensagem = "Turma recuperada com sucesso", Dados = response };

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao recuperar turma por professor: {ex.Message}");
                return new OperacaoResponse<TurmaResponse?> { Sucesso = false, Mensagem = "Erro ao recuperar turma por professor" };
            }
        }
    }
}
