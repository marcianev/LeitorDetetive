using ApiBackend.Helpers;
using ApiBackend.Models;
using ApiBackend.Repostiories;
using ApiBackend.Services.Interface;
using Shared.DTOs.Requests;
using Shared.DTOs.Requests.Auth;
using Shared.DTOs.Responses;
using Shared.DTOs.Responses.Auth;
using System.Diagnostics;

namespace ApiBackend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IProfessorService _professorService;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ICriptoService _criptoService;

        public AuthService(IUsuarioService usuarioService, IJwtService jwtService,
            IProfessorService professorService, ICriptoService criptoService,
            IEmailService emailService)
        {
            _usuarioService = usuarioService;
            _professorService = professorService;
            _jwtService = jwtService;
            _emailService = emailService;
            _criptoService = criptoService;
        }

        public async Task<LoginResponse?> Login(LoginRequest request)
        {
            var usuario = await _usuarioService.GetByIUser(request.User);

            if (usuario == null)
                return null;

            if (request.Senha != request.Senha)
                return null;

            return new LoginResponse
            {
                UsuarioId = usuario.Id,
                User = usuario.User,
                Tipo = usuario.Tipo,
                Token = _jwtService.GerarToken(usuario),
                StatusUsuario = usuario.StatusUsuario,
                StatusSenha = usuario.StatusSenha
            };
        }

        public async Task<bool> RecuperarSenha(RecuperarSenhaRequest request)
        {
            Usuario? usuario = await _usuarioService.GetByIUser(request.User); 

            if (usuario == null)
                return false;

            Professor? professor = await _professorService.GetByUsuarioId(usuario.Id);

            if(professor == null)
                return false;

            if(professor.Email != request.Email) 
                return false;

            string senhaProvisoria = SenhaHelper.GerarSenhaProvisoria(8);
            if (senhaProvisoria == null)
                return false;

            usuario.Senha = _criptoService.GerarHash(senhaProvisoria);
            usuario.StatusSenha = false;

            await _usuarioService.UpdateAsync(usuario);

            bool emailEnviado = await _emailService.EnviarEmail(
               professor.Email,
               "Leitor Detive - Código de Acesso",
               $"Olá, {usuario.User}. " +
               $"\nSeu novo código de acesso é: {senhaProvisoria}. \n" +
               $"Utilize a opção  ACESSO PROVISÓRIO no aplicativo para definir a senha definitiva.\n" +
               $" Atenciosamente\n" +
               $" Equipe Leitor Detetive ");

            if(!emailEnviado)    
                return false;

            return true;
        }

        public async Task<OperacaoResponse> AcessoProvisorio(AcessoProvisorioRequest request)
        {
            try
            {   
                if (request.User == string.Empty)
                    return OperacaoResponse.Resposta(
                        false,
                        "Usuário é obrigatório.");

                if (request.NovaSenha == string.Empty)
                    return OperacaoResponse.Resposta(
                        false,
                        "Nova senha é obrigatória.");

                if (request.SenhaProvisoria == string.Empty)
                    return OperacaoResponse.Resposta(
                       false,
                       "Código é obrigatória.");


                var usuario = await _usuarioService.GetByIUser(request.User);
                if (usuario == null)
                    return OperacaoResponse.Resposta(
                        false,
                       "Usuario não encontrado.");

                var verificacao = _criptoService.VerificarHash(request.SenhaProvisoria, usuario.Senha);

                if (verificacao == false)
                    return OperacaoResponse.Resposta(
                        false,
                        "Código enviado por email não confere.");

                usuario.Senha = _criptoService.GerarHash(request.NovaSenha);
                usuario.StatusSenha = true;                  
             

                var atualizacao = await _usuarioService.UpdateAsync(usuario);

                if (atualizacao == null)
                    return OperacaoResponse.Resposta(
                        false,
                        "Usuario não pode ser atualizado");
                else
                    return OperacaoResponse.Resposta(
                        true,
                        "Nova senha cadastrada.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao cadastrar senha: {ex.Message}");
                return OperacaoResponse.Resposta(
                        false,
                        $"Erro ao cadastrar senha: {ex.Message}");
            }
        }

       
    }
}
