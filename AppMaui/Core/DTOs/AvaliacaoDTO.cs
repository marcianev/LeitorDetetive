using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public class AvaliacaoDTO
    {     
        public int IdAvaliacao { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; } = string.Empty;        
        public StatusAvaliacao Status { get; set; }   
        public DateTime DataCadastro { get; set; }
       
        public int LivroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;       
        public string Ilustrador { get; set; } = string.Empty;   
        public string Capa { get; set; } = string.Empty;
       
        public string Nickname { get; set; } = string.Empty;
       
    }
}
