using AppMaui.Core.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public class EstanteDTO
    {      
        public int IdLivro { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Ilustrador { get; set; } = string.Empty;        
        public string Editora { get; set; } = string.Empty;        
        public string Capa { get; set; } = string.Empty;
        
        public int IdLeitura { get; set; }       
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }       
        public StatusLeitura? Status { get; set; }       
        public int UsuarioId { get; set; }   

        public Color CorLivro
        {
            get
            {
                return Status switch
                {
                    StatusLeitura.Iniciada => (Color)Application.Current!.Resources["palhaMedio"],
                    StatusLeitura.Pausada => (Color)Application.Current!.Resources["begeUltra"],
                    StatusLeitura.Concluida => (Color)Application.Current!.Resources["verdeMedio"],
                    _ => (Color)Application.Current!.Resources["begeUltra"]
                };
            }
        }
    }
}
