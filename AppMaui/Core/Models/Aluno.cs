using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um aluno no sistema.
    /// </summary>
    public class Aluno
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [MaxLength(100), NotNull]
        public string Nome { get; set; } = string.Empty;

        [NotNull]
        public string Nickname { get; set; } = string.Empty;

        public string CodigoAcesso { get; set; } = string.Empty;

        [NotNull]
        public int UsuarioId { get; set; }

        public int TurmaUui { get; set; }

        public int PatenteId { get; set; }
    }
}
