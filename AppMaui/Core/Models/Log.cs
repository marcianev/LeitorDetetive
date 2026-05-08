using AppMaui.Core.Enums;
using SQLite;


namespace AppMaui.Core.Models
{
    public class Log
    {
        //atributos do modelo de log, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
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
