using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public class LivrosMaisLidosDTO
    {
        public string Turma { get; set; } = string.Empty;

        public string TituloLivro { get; set; } = string.Empty;

        public string CapaLivro { get; set; } = string.Empty;

        public int QuantidadeLeituras { get; set; }
    }
}
