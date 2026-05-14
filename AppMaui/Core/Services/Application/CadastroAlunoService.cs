using AppMaui.Core.Data;
using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Local;
using AppMaui.Core.Services.Security;
using AppMaui.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Application
{
    public class CadastroAlunoService
    {
        private readonly UsuarioService _usuarioService;
        private readonly AlunoService _alunoService;        
        private readonly DatabaseService _databaseService;
        private readonly ICriptoService _criptoService;
        private readonly ProfessorService _professorService;
        private readonly TurmaService _turmaService;

        public CadastroAlunoService(UsuarioService usuarioService, AlunoService alunoService,
            ICriptoService criptoService, DatabaseService databaseService, ProfessorService professorService,
            TurmaService turmaService)
        {
            _usuarioService = usuarioService;
            _alunoService = alunoService;
            _databaseService = databaseService;
            _criptoService = criptoService;
            _professorService = professorService;
            _turmaService = turmaService;
        }

        //metodo salvar usuario e aluno
        public async Task<string> CadastrarAluno(CadastroAlunoDTO cadastroAlunoDTO)
        {
            try
            {               
                //validações
                if (string.IsNullOrWhiteSpace(cadastroAlunoDTO.Nome))
                    return "Nome obrigatório";               

                var db = _databaseService.Conexao;

                //gerar senha provisoria
                string senhaProvisoria = SenhaService.GerarCodigoAluno(4);
                cadastroAlunoDTO.CodAcess = senhaProvisoria;
                string hash = _criptoService.GerarHash(cadastroAlunoDTO.CodAcess);
                var (nick, turma) = await GerarNickname(cadastroAlunoDTO.Nome);
                if (turma == 0)
                    turma = 1;

                //cadastra
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
                    CodigoAcesso =cadastroAlunoDTO.CodAcess,
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
        }//fim salvar

        //metodo para gerar nickname do aluno
        public async Task<(string, int)> GerarNickname(string aluno)
        {
            Debug.WriteLine("chegamos no gerarNickname");
            //validação
            if (string.IsNullOrWhiteSpace(aluno))
                return (string.Empty, 0);

            //busca o nome do professor
            var user = SessaoService.UsuarioLogado;
            Professor professor = await _professorService.BuscarProfessorPorUsuario(user.Id);
            if (professor == null)
                return (string.Empty,0);
            
            //aproveitar a pesquisa do professor para registrar o idTurma no dto
            int turma = await _turmaService.BuscarPorProfessor(professor.Id);

            //transforma o nome do aluno e do professor em arrays de caracteres para pegar as iniciais
            char[] n = aluno.ToUpper().ToCharArray();
            char[] p = professor.Nome.ToUpper().ToCharArray();

            //variaveis
            Random rand = new();
            int numm;
            int i = 0;
            string nickname;
            bool nicknameExists;

            //por enquanto funciona, para até 90 repetições
            //preciso pensar em algoritmo para ampliar
            //gerar nickname com as iniciais do nome do aluno, do professor e um número aleatório de 2 dígitos
            do
            {
                numm = rand.Next(10, 99);
                nickname = $"{n[0]}{p[0]}{numm}";
                nicknameExists = await _alunoService.VerificarExistenciaNick(nickname);
                i++;
            } while (nicknameExists && i < 90);
            return (nickname, turma);
        }//fecha método gerar nickname       


    }

}
