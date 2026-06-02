using ApiBackend.Data;
using ApiBackend.Helpers;
using ApiBackend.Models;
using ApiBackend.Services.Interface;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;

namespace ApiBackend.Services
{
    public class CadastroService : ICadastroService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IProfessorService _professorService;
        private readonly IEmailService _emailService;
        private readonly ICriptoService _criptoService;
        private readonly AppDbContext _context;

        public CadastroService(IUsuarioService usuarioService, IProfessorService professorService,
            ICriptoService criptoService, AppDbContext dbContext, IEmailService emailService)
        {
            _usuarioService = usuarioService;
            _professorService = professorService;
            _criptoService = criptoService;
            _context = dbContext;
            _emailService = emailService;

        }

        public async Task<OperacaoResponse> CadastrarProfessor (CadastroProfessorRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nome))
                   return OperacaoResponse.Resposta(
                        false,
                        "Nome obrigatório");
                if (string.IsNullOrWhiteSpace(request.User))
                    return OperacaoResponse.Resposta(
                        false,
                        "Usuário Obrigatório");
                if (string.IsNullOrWhiteSpace(request.Email))
                    return OperacaoResponse.Resposta(
                        false,
                        "Email obrigatório");
                if (string.IsNullOrWhiteSpace(request.Cpf))
                    return OperacaoResponse.Resposta(
                        false,
                        "CPF obrigatório");

                var emailExiste = await _professorService.GetByEmail(request.Email);
                if (emailExiste != null)
                    return OperacaoResponse.Resposta(
                        false,
                        "Email já existente.");

                request.Cpf = request.Cpf.Replace(".", "").Replace("-", "");
                var cpfExiste = await _professorService.CpfExists(request.Cpf);
                if (cpfExiste)
                    return OperacaoResponse.Resposta(
                        false,
                        "CPF já existente");

                var userExiste = await _usuarioService.UserExists(request.User);
                if (userExiste)
                    return OperacaoResponse.Resposta(
                        false,
                        "Usuário já existente.");

                string senhaProvisoria = SenhaHelper.GerarSenhaProvisoria(8);
                string hash = _criptoService.GerarHash(senhaProvisoria);

                var professor = new Professor
                {
                    Nome = request.Nome,
                    Cpf = request.Cpf,
                    Email = request.Email                   
                };    
                var usuario = new Usuario
                {
                    User = request.User,
                    Senha = hash,
                    StatusSenha = false,
                    StatusUsuario = true,
                    Tipo = "Professor"
                };

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    usuario = await _usuarioService.AddAsync(usuario);
                    professor.UsuarioId = usuario.Id;
                    await _professorService.AddAsync(professor);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return OperacaoResponse.Resposta(
                        false,
                        "Erro ao cadastrar professor.");
                } 
                    string assunto = "Bem-vindo ao Leitor Detetive - Sua senha provisória";
                    string mensagem = $"Olá, {request.User}.\n\nSua conta foi criada com sucesso! Sua senha provisória é: " +
                        $"{senhaProvisoria}\n\nPor favor, acesse em PRIMEIRO ACESSO e altere sua senha.\n\nAtenciosamente,\nEquipe Leitor Detetive.";
                    bool resul = await _emailService.EnviarEmail(request.Email, assunto, mensagem);
                if(!resul)
                    return OperacaoResponse.Resposta(
                        false,
                        $"Professor cadastro, sem envio de email.");   
                return OperacaoResponse.Resposta(
                    true,
                    $"Cadastro realizado {request.User}");                
            }
            catch (Exception ex)
            {
                return OperacaoResponse.Resposta(
                        false, 
                        $"Erro ao cadastrar: {ex.Message}");
            }
        }
    }
}
