
using AppMaui.Core.DTOs;
using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;
using AppMaui.Core.Services.External;

namespace AppMaui.Core.Services.Local
{
    /// <summary>
    /// Gerencia operações de alunos incluindo atualização dinâmica de patentes baseada em leituras.
    /// </summary>
    public class AlunoService(AlunoRepository _repositorio, EmailService emailService,
       EventoService eventoService, ProfessorService professorService)
    {
        public async Task<bool> SalvarAluno(Aluno aluno)
        {
            try
            {
                if (string.IsNullOrEmpty(aluno.Nome) || aluno.Nome.Length > 100)
                    return false;

                await _repositorio.Add(aluno);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar aluno por ID: {ex.Message}");
            }
        }

        public async Task<List<Aluno>> ListarAlunos(int turma)
        {
            try
            {
                if (turma <= 0)
                    throw new Exception("Turma inválida.");

                return await _repositorio.GetByTurma(turma);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar alunos: {ex.Message}");
            }
        }

        public async Task<bool> AtualizarAluno(Aluno aluno)
        {
            try
            {
                if (string.IsNullOrEmpty(aluno.Nome) || aluno.Nome.Length > 100 || aluno.Id <= 0)
                    return false;

                await _repositorio.Update(aluno);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar aluno por ID: {ex.Message}");
            }
        }

        public async Task<bool> DeletarAluno(int id)
        {
            try
            {
                var aluno = await _repositorio.GetById(id);
                if (aluno == null)
                    return false;

                await _repositorio.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar aluno: {ex.Message}");
            }
        }

        /// <summary>Busca código de acesso de um aluno pelo ID do usuário.</summary>
        public async Task<string> BuscarAlunoPorUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    return "";

                var aluno = await _repositorio.GetByUsuarioId(usuarioId);
                return aluno?.CodigoAcesso ?? "";
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar aluno por ID: {ex.Message}");
            }
        }

        /// <summary>Busca dados completos de um aluno pelo ID do usuário.</summary>
        public async Task<Aluno?> BuscarAlunoUsuario(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    return null;

                return await _repositorio.GetByUsuarioId(usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar aluno por ID: {ex.Message}");
            }
        }

        /// <summary>Verifica existência de nickname no banco de dados.</summary>
        public async Task<bool> VerificarExistenciaNick(string nick)
        {
            try
            {
                if (string.IsNullOrEmpty(nick))
                    return false;

                return await _repositorio.NicknameExist(nick);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar NickName: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza patente do aluno a cada 3 leituras concluídas (progressão linear).
        /// Registra este evento para notificações e envia um email para o professor
        /// </summary>
        public async Task<bool> AtualizarPatente(int usuarioId, int quantidadeLeituras)
        {
            try
            {
                if (usuarioId <= 0)
                    return false;

                var aluno = await _repositorio.GetByUsuarioId(usuarioId);
                if (aluno == null)
                    return false;

                if (quantidadeLeituras % 3 == 0)
                {
                    int novoNivel = quantidadeLeituras / 3;
                    aluno.PatenteId = await _repositorio.GetIdPatente(novoNivel + 1, aluno.PatenteId);
                    EventoSistema eventoSistema = new()
                    {
                        Tabela = "Aluno",
                                                
                        Descricao = "Subiu de nível.",
                        ReferenciaId = aluno.Id,
                        UsuarioId = aluno.UsuarioId
                    };
                    await eventoService.SalvarEvento(eventoSistema);
                    var professor = await professorService.BuscarPorTurma(aluno.TurmaUui);
                    if(professor == null)
                        return false;

                    await emailService.EnviarEmail(
                        professor.Email,
                        "Novo Nível",
                        $"Ois, passando aqui pra avisar que temos novidade na sua turma." +
                        $"{aluno.Nome} agora tem uma nova patente. Visite Leitor Detetite para " +
                        $"conferir.");                    
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar patente: {ex.Message}");
            }
        }

        /// <summary>Gera dashboard do aluno com patente, leituras e última avaliação.</summary>
        public async Task<DashBoardADTO?> GerarDashBoard(int usuarioId)
        {
            try
            {
                if (usuarioId <= 0)
                    return null;

                return await _repositorio.GerarDashBoard(usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar DTO: {ex.Message}");
            }
        }

        /// <summary>Gera dashboard do professor com alunos, patentes e total de leituras por turma.</summary>
        public async Task<List<DashBoardPDTO>?> GerarDashBoardProfessor(int idProfessor)
        {
            try
            {
                if (idProfessor <= 0)
                    return null;

                return await _repositorio.GerarDashBoardP(idProfessor);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar DTO: {ex.Message}");
            }
        }

       ///<summary>
       ///Gera dados para pagina de Alunos do perfil do professor
       ///Com patente, nickname, nome, codigo de acesso, capa do livro atual e quantidade leituras concluidas
       ///</summary>
       public async Task<List<AlunoPDTO>?> BuscarDadosAlunoP(int idTurma)
        {
            try
            {              
                if (idTurma <= 0)
                    return null;               
                return await _repositorio.BuscarDadosAlunoP(idTurma);
                throw new Exception("CHEGAMOS");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao gerar DTO: {ex.Message}");
            }
        }
    }
}