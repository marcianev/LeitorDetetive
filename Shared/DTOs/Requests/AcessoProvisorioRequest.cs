using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Requests
{
    public class AcessoProvisorioRequest
    {
        public string User { get; set; } = string.Empty;

        public string SenhaProvisoria { get; set; } = string.Empty;

        public string NovaSenha { get; set; } = string.Empty;
    }
}
