using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public class CadastroProfessorDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string SenhaProvisoria { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public bool? StatusSenha { get; set; }
        public bool? StatusUser { get; set; }
    }
}
