using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um caminho de aprendizado estruturado no sistema.
    /// </summary>
    public class Trilha
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;
    }
}
