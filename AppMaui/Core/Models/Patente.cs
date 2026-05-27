using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um nível de conquista ou badge dentro de uma trilha de aprendizado.
    /// </summary>
    public class Patente
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [NotNull]
        public int Nivel { get; set; }

        [NotNull]
        public int TrilhaId { get; set; }
    }
}
