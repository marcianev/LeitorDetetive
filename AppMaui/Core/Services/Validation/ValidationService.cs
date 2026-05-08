using AppMaui.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Validation
{
    public class ValidationService : IValidationService
    {
        public bool ValidarCPF(string cpf)
        {

            //validações
            if (string.IsNullOrEmpty(cpf))
                return false;
            cpf = cpf.Replace(".", "").Replace("-", "");//remover máscara antes de validar extensão
            if (cpf.Length != 11)
                return false;
            if (!cpf.All(char.IsDigit)) //verifica se todos os valores são númericos
                return false;
            if (cpf.All(c => c == cpf[0]))  //testar se todos os digitos são iguais
                return false;

            //valida o cpf pelo calculo dos digitos
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
        }//fim cpf

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

        }//fim email

        //calcula validador dos digitos
        private static int CalcularDigito(string cpf, int limite)
        {
            int numCpf = 0;

            //soma os digitos dentro do limite passado
            for (int i = 0; i < limite - 1; i++)
            {
                int mul = (cpf[i] - '0') * (limite - i);
                numCpf += mul;
            }

            //mod 11
            int mod = numCpf % 11;

            if (mod < 2)
                return 0;
            else
                return 11 - mod;
        }
    }
}
