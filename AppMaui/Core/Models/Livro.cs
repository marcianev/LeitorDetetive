using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um livro disponível no sistema com metadados de publicação.
    /// </summary>
    public class Livro
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [NotNull, MaxLength(100)]
        public string Autor { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Ilustrador { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Editora { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Capa { get; set; } = string.Empty;
    }
}
