using ApiBackend.Helpers;
using ApiBackend.Models;
using ApiBackend.Repostiories;
using ApiBackend.Services.Interface;
using Shared.DTOs.Requests;
using Shared.DTOs.Requests.Auth;
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
            var usuario = await _usuarioService.BuscarPorUser(request.User);

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
            Usuario? usuario = await _usuarioService.BuscarPorUser(request.User); 

            if (usuario == null)
                return false;

            Professor? professor = await _professorService.BuscarPorUsuarioId(usuario.Id);

            if(professor == null)
                return false;

            if(professor.Email != request.Email) 
                return false;

            string senhaProvisoria = SenhaHelper.GerarSenhaProvisoria(8);
            if (senhaProvisoria == null)
                return false;

            usuario.Senha = _criptoService.GerarHash(senhaProvisoria);
            usuario.StatusSenha = false;

            await _usuarioService.Atualizar(usuario);

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
    }
}
