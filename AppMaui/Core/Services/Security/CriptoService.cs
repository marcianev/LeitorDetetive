using AppMaui.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Security
{
    public class CriptoService : ICriptoService
    {
        //gerar criptografia da senha
        public string GerarHash(string senha)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(senha),                
                salt,
                10000,
                HashAlgorithmName.SHA256,
                32
            );

            return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
        }

        //verificar se a senha digitada corresponde à senha armazenada
        public bool VerificarHash(string senhaDigitada, string hashSalvo)
        {
           
            var partes = hashSalvo.Split('.');

            byte[] salt = Convert.FromBase64String(partes[0]);
            byte[] hashOriginal = Convert.FromBase64String(partes[1]);

            byte[] hashDigitado = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(senhaDigitada),
                salt,
                10000,
                HashAlgorithmName.SHA256,
                32
            );
            var resultado = CryptographicOperations.FixedTimeEquals(hashDigitado, hashOriginal);            

            return resultado;
        }
    }
}
