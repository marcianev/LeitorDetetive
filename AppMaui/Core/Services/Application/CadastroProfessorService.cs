using AppMaui.Core.Data;
using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Local;
using AppMaui.Core.Services.Security;
using AppMaui.Services;
using AppMaui.Services.Interfaces;
using System.Diagnostics;

namespace AppMaui.Core.Services.Application
{
    public class CadastrarProfessorService
    {
        private readonly UsuarioService _usuarioService;
        private readonly ProfessorService _professorService;
        private readonly EmailService _emailService;
        private readonly DatabaseService _databaseService;
        private readonly ICriptoService _criptoService;


        public CadastrarProfessorService(UsuarioService usuarioService, ProfessorService professorService, 
            ICriptoService criptoService, DatabaseService databaseService, EmailService es)
        {
            _usuarioService = usuarioService;
            _professorService = professorService;
            _emailService = es;
            _databaseService = databaseService;
            _criptoService = criptoService;
        }

        //metodo salvar usuario e professor
        public async Task<string> CadastrarProfessor(CadastroProfessorDTO cadastroProfessorDTO)       
        {         
            try
            {       
                //validações
                if (string.IsNullOrWhiteSpace(cadastroProfessorDTO.User))
                    return "Usuário obrigatório";
                if (string.IsNullOrWhiteSpace(cadastroProfessorDTO.Nome))
                    return "Nome obrigatório";
                if (string.IsNullOrWhiteSpace(cadastroProfessorDTO.Email))
                    return "Email obrigatório";
                if (string.IsNullOrWhiteSpace(cadastroProfessorDTO.Cpf))
                    return "CPF obrigatório";
                
                var db = _databaseService.Conexao;

                //gerar senha provisoria
                string senhaProvisoria = SenhaService.GerarSenhaProvisoria(8);                
                cadastroProfessorDTO.Senha = senhaProvisoria;
                string hash = _criptoService.GerarHash(cadastroProfessorDTO.Senha);
               
                //cadastra
                var usuario = new Usuario
                {
                    User = cadastroProfessorDTO.User,
                    Senha = hash,
                    StatusSenha = false,
                    StatusUsuario = true,
                    Tipo = "Professor"
                };

                var professor = new Professor
                {
                    Nome = cadastroProfessorDTO.Nome,
                    Cpf = cadastroProfessorDTO.Cpf,
                    Email = cadastroProfessorDTO.Email                    
                };

                //envia email com senha provisoria                
                string assunto = "Bem-vindo ao Ajolede - Sua senha provisória";
                string mensagem = $"Olá {cadastroProfessorDTO.User},\n\nSua conta foi criada com sucesso! Sua senha provisória é: " +
                    $"{senhaProvisoria}\n\nPor favor, acesse em PRIMEIRO ACESSO e altere sua senha.\n\nAtenciosamente,\nEquipe Ajolede";
                bool resul = await _emailService.EnviarEmail(cadastroProfessorDTO.Email, assunto, mensagem);
                
               

                await db.RunInTransactionAsync(tran =>
                {
                    tran.Insert(usuario);
                    professor.UsuarioId = usuario.Id;
                    tran.Insert(professor);
                });
                return "Cadastro realizado";
            }
            catch (Exception ex)
            {
                return $"Erro ao cadastrar: {ex.Message}";
            }   
        }        
    }
}
