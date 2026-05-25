
namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO utilizado para consolidar e transportar os dados
    /// exibidos na área de mais lidos da pagina de turma
    /// para o perfil professor
    /// </summary>
    public class LivrosMaisLidosDTO
    {
        public string Turma { get; set; } = string.Empty;
        public string TituloLivro { get; set; } = string.Empty;
        public string CapaLivro { get; set; } = string.Empty;
        public int QuantidadeLeituras { get; set; }
    }
}
