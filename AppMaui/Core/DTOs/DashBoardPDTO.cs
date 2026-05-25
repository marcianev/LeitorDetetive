using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public class DashBoardPDTO
    {
        public string Aluno { get; set; } = string.Empty;
        public string Patente { get; set; } = string.Empty;
        public int QuantidadeLeituras { get; set; }
        public int NivelUp { get; set; }
        public int Leituras015 { get; set; }
    }
}
