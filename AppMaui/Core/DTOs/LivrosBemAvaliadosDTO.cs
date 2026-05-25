
namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO utilizado para consolidar e transportar os dados
    /// exibidos na área de bem avaliados da pagina de turma
    /// para o perfil professor
    /// </summary>
    public class LivrosBemAvaliadosDTO
    {
        public string Turma { get; set; } = string.Empty;
        public string Capa { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public int Nota { get; set; }
    }
}
