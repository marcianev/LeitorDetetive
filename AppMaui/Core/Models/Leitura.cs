using AppMaui.Core.Enums;
using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa o registro de leitura de um livro por um usuário.
    /// Rastreia o progresso e status da leitura.
    /// </summary>
    public class Leitura
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        [NotNull, MaxLength(10)]
        public StatusLeitura Status { get; set; }

        [NotNull]
        public int UsuarioId { get; set; }

        [NotNull]
        public int LivroId { get; set; }
    }
}
