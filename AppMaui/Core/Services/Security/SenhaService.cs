using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Local;
using AppMaui.Services.Interfaces;
using System.Diagnostics;
using System.Security.Cryptography;


namespace AppMaui.Core.Services.Security
{
    /// <summary>
    /// Serviço para geração e atualização de senhas com suporte a criptografia.
    /// </summary>
    public class SenhaService
    {
        UsuarioService _usuarioService;
        ICriptoService _criptoService;
        EmailService _emailService;

        public SenhaService(UsuarioService usuarioservice, ICriptoService criptoService,
            EmailService emailService)
        {
            _usuarioService = usuarioservice;
            _criptoService = criptoService;
            _emailService = emailService;
        }

        /// <summary>Gera uma senha provisória com caracteres minúsculos, maiúsculos, números e especiais.</summary>
        public static string GerarSenhaProvisoria(int tam)
        {
            const string min = "abcdefghijklmnopqrstuvwxyz";
            const string mai = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string num = "0123456789";
            const string esp = "!@#$%&*";
            string todos = min + mai + num + esp;

            var senha = new char[tam];

            using (var rng = RandomNumberGenerator.Create())
            {
                senha[0] = SortearCaractere(rng, min);
                senha[1] = SortearCaractere(rng, mai);
                senha[2] = SortearCaractere(rng, num);
                senha[3] = SortearCaractere(rng, esp);

                for (int i = 4; i < tam; i++)
                {
                    senha[i] = SortearCaractere(rng, todos);
                }
            }

            var senhaProvisoria = senha.OrderBy(c => Guid.NewGuid()).ToArray();
            var s = new String(senhaProvisoria);

            return s;
        }

        /// <summary>Gera um código de acesso numérico para alunos.</summary>
        public static string GerarCodigoAluno(int tam)
        {
            Debug.WriteLine("chegamos no gerar codigoAluno");
            const string chars = "0123456789";
            var codigo = new char[tam];
            using (var rng = RandomNumberGenerator.Create())
            {
                for (int i = 0; i < tam; i++)
                {
                    codigo[i] = SortearCaractere(rng, chars);
                }
            }
            return new String(codigo);
        }

        /// <summary>Sorteia um caractere aleatório de um conjunto usando criptografia segura.</summary>
        private static char SortearCaractere(RandomNumberGenerator rdm, string chars)
        {
            byte[] guardaByte = new byte[1];
            rdm.GetBytes(guardaByte);
            int index = guardaByte[0] % chars.Length;
            return chars[index];
        }

        /// <summary>Define uma nova senha definitiva após validação da senha provisória.</summary>
        public async Task<string> NovaSenhaDefinitiva(CadastroProfessorDTO cp)
        {
            try
            {
                if (cp.User == string.Empty)
                    return "Usuário é obrigatório.";
                if (cp.Senha == string.Empty)
                    return "Nova senha é obrigatória.";

                var usuario = await _usuarioService.BuscarUsuarioPorUser(cp.User);               
                if (usuario == null)
                    return "Usuario não encontrado.";

                var sp = _criptoService.VerificarHash(cp.SenhaProvisoria, usuario.Senha);   
                if (sp == false)
                    return "Código enviado por email não confere.";

                var u = new Usuario
                {
                    Id = usuario.Id,
                    User = usuario.User,
                    Senha = _criptoService.GerarHash(cp.Senha),
                    StatusSenha = true,
                    StatusUsuario = true,
                    Tipo = usuario.Tipo
                };

                var au = await _usuarioService.AtualizarUsuario(u);

                if (au == false)
                    return "Usuario não pode ser atualizado";                

                return "Nova senha cadastrada.";
            }
            catch (Exception ex)
            {
                return $"Erro ao cadastrar senha: {ex.Message}";
            }
        }

        /// <summary>Gera uma nova senha provisória para professor (apenas professores podem executar).</summary>
        public async Task<string> NovaSenhaProvisoria(CadastroProfessorDTO cp)
        {
            try
            {
                if (string.IsNullOrEmpty(cp.Tipo))
                    return "Tipo de usuário é obrigatório";
                if (cp.Tipo != "Professor")
                    return "Somente professor pode alterar senhas.";
                if (cp.Id <= 0)
                    return "Usuário inválido.";
                if (await _usuarioService.BuscarUsuarioPorId(cp.Id) == null)
                    return "Usuário não encontrado.";
                if (string.IsNullOrEmpty(cp.Nome))
                    return "Nome de usuario é obrigatório.";
                if (cp.StatusSenha == null)
                    return "Status da senha é obrigatório.";
                

                string senhaProvisoria = GerarSenhaProvisoria(8);

                var usuario = new Usuario
                {
                    Id = cp.Id,
                    User = cp.User,
                    Senha = _criptoService.GerarHash(senhaProvisoria),
                    StatusSenha = false,
                    Tipo = "Professor"
                };

                await _usuarioService.AtualizarUsuario(usuario);


                return "Nova senha enviada para seu email.";
            }
            catch (Exception ex)
            {
                return $"Erro ao gerar nova senha: {ex.Message}";
            }
        }
    }
}
