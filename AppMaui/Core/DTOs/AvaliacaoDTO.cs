using AppMaui.Core.Enums;

namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO utilizado para exibição de avaliações
    /// juntamente com dados do livro e do aluno.
    /// </summary>
    public class AvaliacaoDTO
    {     
        //dados da avaliação
        public int IdAvaliacao { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; } = string.Empty;        
        public StatusAvaliacao Status { get; set; }   

        //dados do livro avaliado
        public DateTime DataCadastro { get; set; }        
        public int LivroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;       
        public string Ilustrador { get; set; } = string.Empty;   
        public string Capa { get; set; } = string.Empty;       

        //dados do aluno responsável pela avaliação
        public string Nickname { get; set; } = string.Empty;
        public int UsuarioId { get; set; }

        //define a cor da borda para indicar status da moderação
        public Color BordaComentario
        {
            get
            {
                return Status switch
                {
                    StatusAvaliacao.AnaliseAutomatica => (Color)Application.Current!.Resources["vermelhoMedio"],
                    StatusAvaliacao.AnaliseManual => (Color)Application.Current!.Resources["vermelho"],
                    StatusAvaliacao.Aprovada => (Color)Application.Current!.Resources["verdeMedio"],
                    _ => (Color)Application.Current!.Resources["begeUltra"]
                };
            }
        } 
    }
}
