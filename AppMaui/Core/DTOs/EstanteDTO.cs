using AppMaui.Core.Enums;

namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO utilizado para consolidar e transportar os dados
    /// exibidos na estante do aluno
    /// </summary>   
    public class EstanteDTO
    {      
        //dados vinculados a entidade livro
        public int IdLivro { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Ilustrador { get; set; } = string.Empty;        
        public string Editora { get; set; } = string.Empty;        
        public string Capa { get; set; } = string.Empty;
        
        //dados vinculados a entidade leitura
        public int IdLeitura { get; set; }       
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }       
        public StatusLeitura? Status { get; set; }       
        public int UsuarioId { get; set; }   

        // alterar a cor do background para vizualizar o status de leitura       
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
