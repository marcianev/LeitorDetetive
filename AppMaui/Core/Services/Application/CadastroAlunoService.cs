using AppMaui.Core.Data;
using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services.Local;
using AppMaui.Core.Services.Security;
using AppMaui.Services.Interfaces;
using System.Diagnostics;

namespace AppMaui.Core.Services.Application
{
    /// <summary>
    /// Orquestra o cadastro de alunos, gerando credenciais, nickname e registrando usuário e aluno no banco.
    /// </summary>
    public class CadastroAlunoService
    {       
        private readonly AlunoService _alunoService;        
        private readonly DatabaseService _databaseService;
        private readonly ICriptoService _criptoService;
        private readonly ProfessorService _professorService;
        private readonly TurmaService _turmaService;

        public CadastroAlunoService(AlunoService alunoService,
            ICriptoService criptoService, DatabaseService databaseService, ProfessorService professorService,
            TurmaService turmaService)
        {           
            _alunoService = alunoService;
            _databaseService = databaseService;
            _criptoService = criptoService;
            _professorService = professorService;
            _turmaService = turmaService;
        }

        /// <summary>Cadastra um novo aluno gerando nickname, senha provisória e registrando em transação.</summary>
        public async Task<string> CadastrarAluno(CadastroAlunoDTO cadastroAlunoDTO)
        {
            try
            {               
                if (string.IsNullOrWhiteSpace(cadastroAlunoDTO.Nome))
                    return "Nome obrigatório";               

                var db = _databaseService.Conexao;

                string senhaProvisoria = SenhaService.GerarCodigoAluno(4);
                cadastroAlunoDTO.CodAcess = senhaProvisoria;
                string hash = _criptoService.GerarHash(cadastroAlunoDTO.CodAcess);
                var (nick, turma) = await GerarNickname(cadastroAlunoDTO.Nome);
                if (turma == 0)
                    turma = 1;

                var usuario = new Usuario
                {
                    User = nick,
                    Senha = hash,
                    StatusSenha = false,
                    StatusUsuario = true,
                    Tipo = "Aluno"                   
                };

                var aluno = new Aluno
                {
                    Nome = cadastroAlunoDTO.Nome,
                    TurmaId = turma,
                    PatenteId = 1,
                    CodigoAcesso = cadastroAlunoDTO.CodAcess,
                    Nickname = nick
                };               

                await db.RunInTransactionAsync(tran =>
                {
                    tran.Insert(usuario);
                    aluno.UsuarioId = usuario.Id;
                    tran.Insert(aluno);
                });
                Debug.WriteLine("cadastrado com sucesso");
                return "Cadastro realizado";
            }
            catch (Exception ex)
            {
                return $"Erro ao cadastrar: {ex.Message}";
            }
        }

        /// <summary>
        /// Gera nickname único combinando iniciais do aluno e professor com número aleatório.
        /// Limita até 90 tentativas para evitar loops infinitos.
        /// </summary>
        public async Task<(string, int)> GerarNickname(string aluno)
        {
            Debug.WriteLine("chegamos no gerarNickname");
            if (string.IsNullOrWhiteSpace(aluno))
                return (string.Empty, 0);

            var user = SessaoService.UsuarioLogado;
            if(user == null)
                return (string.Empty, 0);
            Professor? professor = await _professorService.BuscarProfessorPorUsuario(user.Id);
            if (professor == null)
                return (string.Empty, 0);

            int turma = await _turmaService.BuscarPorProfessor(professor.Id);

            char[] n = aluno.ToUpper().ToCharArray();
            char[] p = professor.Nome.ToUpper().ToCharArray();

            Random rand = new();
            int numm;
            int i = 0;
            string nickname;
            bool nicknameExists;

            do
            {
                numm = rand.Next(10, 99);
                nickname = $"{n[0]}{p[0]}{numm}";
                nicknameExists = await _alunoService.VerificarExistenciaNick(nickname);
                i++;
            } while (nicknameExists && i < 90);
            return (nickname, turma);
        }
    }
}


