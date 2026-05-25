
namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO utilizado para consolidar e transportar os dados
    /// exibidos no dashboard do perfil Aluno
    /// </summary>
    public class DashBoardADTO
    {
        //dados vinculados a entidade aluno
        public string Patente { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string MensagemPatente { get; set; } = string.Empty; 

        //dado vinculdado a entidade leitura
        public int QuantidadeLeitura { get; set; }

        //dado vinculado a entidade livro
        public string Titulo { get; set; } = string.Empty;
        public string Capa { get; set; } = string.Empty;

        //dado vinculado a entidade avaliacao
        public string Comentario { get; set; } = string.Empty;        
    }
}
