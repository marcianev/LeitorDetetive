using System.Diagnostics;
using System.Security.Cryptography;

namespace ApiBackend.Helpers
{
    public class SenhaHelper
    {    
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
        }
    }
