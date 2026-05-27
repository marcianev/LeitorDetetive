using SQLite;


namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um professor no sistema com dados de identificação e acesso.
    /// </summary>
    public class Professor
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [NotNull, MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [NotNull, MaxLength(15)]
        public string Cpf { get; set; } = string.Empty;

        [NotNull]
        public int UsuarioId { get; set; }
    }
}
