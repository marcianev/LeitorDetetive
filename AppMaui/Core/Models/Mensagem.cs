using AppMaui.Core.Enums;
using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um template de mensagem do sistema categorizável por tipo.
    /// </summary>
    public class Mensagem
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull, MaxLength(20)]
        public TipoMensagem Tipo { get; set; }

        [NotNull, MaxLength(100)]
        public string Conteudo { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;
    }
}
