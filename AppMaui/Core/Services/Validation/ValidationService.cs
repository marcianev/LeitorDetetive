using AppMaui.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Validation
{
    /// <summary>
    /// Serviço para validação de dados como CPF e email com suporte a padrões específicos.
    /// </summary>
    public class ValidationService : IValidationService
    {
        /// <summary>Valida CPF com verificação de dígitos verificadores e formato.</summary>
        public bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            if (!cpf.All(char.IsDigit))
                return false;
            if (cpf.All(c => c == cpf[0]))
                return false;

            int validador1 = CalcularDigito(cpf, 10);
            int validador2 = CalcularDigito(cpf, 11);

            if (validador1 == (cpf[9] - '0') && validador2 == (cpf[10] - '0'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Valida email com padrão regex básico.</summary>
        public bool ValidarEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;
                email = email.Trim();

                string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

                return Regex.IsMatch(email, padrao);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Calcula dígito verificador do CPF usando algoritmo módulo 11.</summary>
        private static int CalcularDigito(string cpf, int limite)
        {
            int numCpf = 0;

            for (int i = 0; i < limite - 1; i++)
            {
                int mul = (cpf[i] - '0') * (limite - i);
                numCpf += mul;
            }

            int mod = numCpf % 11;

            if (mod < 2)
                return 0;
            else
                return 11 - mod;
        }

        ///<summary>Verificar nome e pelo menos 1 sobrenome, nome com pelo menos 3 letras 
        ///nome com somente letras
        ///</summary>
        public bool ValidarNome(string nomeCompleto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                return false;

            var partes = nomeCompleto
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // nome e sobrenome
            if (partes.Length < 2)
                return false;

            // Cada parte deve ter pelo menos 2 letras
            foreach (var parte in partes)
            {
                if (parte.Length < 2)
                    return false;

                if (!parte.All(char.IsLetter))
                    return false;
            }

            return true;
        }
    }
}
