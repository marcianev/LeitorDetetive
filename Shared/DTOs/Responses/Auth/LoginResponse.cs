using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Responses.Auth
{
    public class LoginResponse
    {
        public int UsuarioId { get; set; }
        public string User { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public bool StatusSenha { get; set; }
        public bool StatusUsuario { get; set; }
    }
}
