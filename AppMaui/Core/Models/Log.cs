using AppMaui.Core.Enums;
using SQLite;


namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um registro de auditoria rastreando alterações nas tabelas do sistema.
    /// </summary>
    public class Log
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull]
        public string Tabela { get; set; } = string.Empty;

        [NotNull, MaxLength(10)]
        public AcaoLog Acao { get; set; }

        [NotNull, MaxLength(50)]
        public string Campo { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ValorAnterior { get; set; } = string.Empty;

        [MaxLength(100)]
        public string ValorNovo { get; set; } = string.Empty;

        [NotNull]
        public DateTime DataHora { get; set; }

        [NotNull]
        public int UsuarioId { get; set; }
    }
}
