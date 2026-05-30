using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public class AlunoPDTO
    {
        public string Patente { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string CodigoAcesso { get; set; } = string.Empty;
        public string? CapaLivroAtual { get; set; }
        public int QuantidadeLivrosLidos { get; set; }
    }
}
