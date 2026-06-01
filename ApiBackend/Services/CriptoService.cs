using ApiBackend.Services.Interface;
using System.Security.Cryptography;
using System.Text;

namespace ApiBackend.Services
{
    public class CriptoService : ICriptoService
    {
        /// <summary>Gera hash seguro de uma senha usando PBKDF2-SHA256 com salt aleatório de 16 bytes.</summary>
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

        /// <summary>Verifica se uma senha digitada corresponde ao hash armazenado usando comparação de tempo constante.</summary>
        public bool VerificarHash(string senhaDigitada, string hashSalvo)
        {
            if (string.IsNullOrWhiteSpace(hashSalvo))
                return false;

            var partes = hashSalvo.Split('.');

            if (partes.Length != 2)
                return false;

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
