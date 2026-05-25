using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public class LivrosBemAvaliadosDTO
    {
        public string Turma { get; set; } = string.Empty;
        public string Capa { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public int Nota { get; set; }
    }
}
