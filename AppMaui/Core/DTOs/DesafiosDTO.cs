using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public class DesafiosDTO
    {
        public int IdDesafio { get; set; }
        public int IdLivro { get; set; }
        public int IdAluno { get; set; }
        public int IdResposta { get; set; }     
        public string Pergunta { get; set; } = string.Empty;
        public string RespostaCorreta { get; set; } = string.Empty;           
        public string Palavra { get; set; } = string.Empty;
        public ObservableCollection<LetraDTO> Letras { get; set; } = [];
    }
}
