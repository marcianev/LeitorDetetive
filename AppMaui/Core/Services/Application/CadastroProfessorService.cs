using AppMaui.Core.Data;
using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Local;
using AppMaui.Core.Services.Security;
using AppMaui.Services.Interfaces;

namespace AppMaui.Core.Services.Application
{
    /// <summary>
    /// Orquestra o cadastro e atualização de professores, gerando credenciais e enviando email de boas-vindas.
    /// </summary>
    public class CadastrarProfessorService
    {
        private readonly ProfessorService _professorService;
        private readonly EmailService _emailService;
        private readonly DatabaseService _databaseService;
        private readonly ICriptoService _criptoService;

        public CadastrarProfessorService(ProfessorService professorService, 
            ICriptoService criptoService, DatabaseService databaseService, EmailService es)
        {
            _professorService = professorService;
            _emailService = es;
            _databaseService = databaseService;
            _criptoService = criptoService;
        }

        /// <summary>Cadastra novo professor ou atualiza dados de um existente. Envia email com senha provisória.</summary>
        public async Task<string> CadastrarProfessor(CadastroProfessorDTO cadastroProfessorDTO)       
        {         
            try
            {       
                if (string.IsNullOrWhiteSpace(cadastroProfessorDTO.Nome))
                    return "Nome obrigatório";
                if (string.IsNullOrWhiteSpace(cadastroProfessorDTO.User))
                    return "Usuário Obrigatório";
                if (string.IsNullOrWhiteSpace(cadastroProfessorDTO.Email))
                    return "Email obrigatório";
                if (string.IsNullOrWhiteSpace(cadastroProfessorDTO.Cpf))
                    return "CPF obrigatório";

                var emailExiste = await _professorService.EmailExiste(cadastroProfessorDTO.Email);
                if (emailExiste)
                    return "Email já cadastrado.";

                cadastroProfessorDTO.Cpf = cadastroProfessorDTO.Cpf.Replace(".", "").Replace("-", "");
                var cpfExiste = await _professorService.CPFExiste(cadastroProfessorDTO.Cpf);
                if (cpfExiste)
                    return "CPF já cadastrado";

                var professor = new Professor
                {
                    Nome = cadastroProfessorDTO.Nome,
                    Cpf = cadastroProfessorDTO.Cpf,
                    Email = cadastroProfessorDTO.Email
                };

                if (cadastroProfessorDTO.Id > 0)
                {
                    professor.Id = cadastroProfessorDTO.Id;
                    professor.UsuarioId = cadastroProfessorDTO.UsuarioId;

                    var res = _professorService.AtualizarProfessor(professor);
                    if (res == null)
                        return "Erro ao atualizar professor.";
                    else
                        return "Professor atualizada com sucesso.";
                }
                else
                {
                    var db = _databaseService.Conexao;

                    string senhaProvisoria = SenhaService.GerarSenhaProvisoria(8);
                    cadastroProfessorDTO.Senha = senhaProvisoria;
                    string hash = _criptoService.GerarHash(cadastroProfessorDTO.Senha);

                    var usuario = new Usuario
                    {
                        User = cadastroProfessorDTO.User,
                        Senha = hash,
                        StatusSenha = false,
                        StatusUsuario = true,
                        Tipo = "Professor"
                    };

                    // Executa o cadastro do usuário e do professor em uma única transação
                    // para garantir consistência dos dados em caso de falha durante o processo 
                    await db.RunInTransactionAsync(tran =>
                    {
                        tran.Insert(usuario);
                        professor.UsuarioId = usuario.Id;
                        tran.Insert(professor);
                    });
                    string assunto = "Bem-vindo ao Leitor Detetive - Sua senha provisória";
                    string mensagem = $"Olá, {cadastroProfessorDTO.User}.\n\nSua conta foi criada com sucesso! Sua senha provisória é: " +
                        $"{senhaProvisoria}\n\nPor favor, acesse em PRIMEIRO ACESSO e altere sua senha.\n\nAtenciosamente,\nEquipe Leitor Detetive.";
                    bool resul = await _emailService.EnviarEmail(cadastroProfessorDTO.Email, assunto, mensagem);
                    return $"Cadastro realizado {cadastroProfessorDTO.User}";
                }     
            }
            catch (Exception ex)
            {
                return $"Erro ao cadastrar: {ex.Message}";
            }   
        }        
    }
}
