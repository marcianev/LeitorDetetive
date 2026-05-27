using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um usuário no sistema com credenciais e status de acesso.
    /// </summary>
    public class Usuario
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull, MaxLength(50), Unique]
        public string User { get; set; } = string.Empty;

        [NotNull]
        public string Senha { get; set; } = string.Empty;

        [NotNull]
        public bool? StatusSenha { get; set; }

        [NotNull]
        public bool? StatusUsuario { get; set; }

        [NotNull]
        public DateTime DataCadastro { get; set; }

        [NotNull]
        public string Tipo { get; set; } = string.Empty;
    }
}
