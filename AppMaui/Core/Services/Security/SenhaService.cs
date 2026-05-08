using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services.Local;
using AppMaui.Services.Interfaces;
using System.Security.Cryptography;


namespace AppMaui.Core.Services.Security
{
    public class SenhaService
    {
        UsuarioService _usuarioService;
        ICriptoService _criptoService;

        public SenhaService(UsuarioService usuarioservice, ICriptoService criptoService)
        {
            _usuarioService = usuarioservice;
            _criptoService = criptoService;
        }
        //gerar a senha provisória
        public static string GerarSenhaProvisoria(int tam)
        {
            //prepara as strings para gerar a senha aleatória
            const string min = "abcdefghijklmnopqrstuvwxyz";
            const string mai = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string num = "0123456789";
            const string esp = "!@#$%&*()_+";
            string todos = min + mai + num + esp;

            //armazena a senha
            var senha = new char[tam];


            using (var rng = RandomNumberGenerator.Create())
            {
                //gera os valores obrigatórios para a senha
                senha[0] = SortearCaractere(rng, min);
                senha[1] = SortearCaractere(rng, mai);
                senha[2] = SortearCaractere(rng, num);
                senha[3] = SortearCaractere(rng, esp);

                //gera os caracteres restantes da senha
                for (int i = 4; i < tam; i++)
                {
                    senha[i] = SortearCaractere(rng, todos);
                }
            }

            //embaralha os caracteres da senha para garantir a aleatoriedade
            var senhaProvisoria = senha.OrderBy(c => Guid.NewGuid()).ToArray();
            var s = new String(senhaProvisoria);
            
            return s;
        }// fim gerar

        //gerar codigo de acesso para aluno
        public static string GerarCodigoAluno(int tam)
        {
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
        }//fim gerar codigo

        //metodo sorteia o caractere aleatório
        private static char SortearCaractere(RandomNumberGenerator rdm, string chars)
        {
            byte[] guardaByte = new byte[1];
            rdm.GetBytes(guardaByte);
            int index = guardaByte[0] % chars.Length;
            return chars[index];
        }

        //nova senha definitiva
        public async Task<string> NovaSenhaDefinitiva(CadastroProfessorDTO cp)
        {
            try
            {
                //validações
                if (cp.User == string.Empty)
                    return "Usuário é obrigatório.";
                if (cp.Senha == string.Empty)
                    return "Nova senha é obrigatória.";
                
                //encontra usuario
                var usuario = await _usuarioService.BuscarUsuarioPorUser(cp.User);               
                if (usuario == null)
                    return "Usuario não encontrado.";                
                
                var sp = _criptoService.VerificarHash(cp.SenhaProvisoria, usuario.Senha);   
                if (sp == false)
                    return "Código enviado por email não confere.";
                

                //depois implentar se StatusSenha for false                
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
        }//fim Nova senha definitiva

        //nova senha provisoria
        public async Task<string> NovaSenhaProvisoria(CadastroProfessorDTO cp)
        {
            try
            {
                //validações
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
                if (string.IsNullOrEmpty(cp.Tipo))
                    return "Tipo de usuário é obrigatório";
               
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
        }//fim Nova senha definitiva*/
    }
}
