using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Responses
{
    public class OperacaoResponse
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;


        public static OperacaoResponse Resposta(bool sucesso, string mensagem)
        {
            return new OperacaoResponse
            {
                Sucesso = sucesso,
                Mensagem = mensagem
            };

        }
    }
}
