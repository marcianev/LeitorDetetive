using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public class DashBoardADTO
    {
        public string Patente { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string MensagemPatente { get; set; } = string.Empty; 
        public int QuantidadeLeitura { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Capa { get; set; } = string.Empty;
        public string Comentario { get; set; } = string.Empty;        
    }
}
