using AppMaui.Core.Enums;
using SQLite;

namespace AppMaui.Core.Models
{
    public class Notificacao
    {
        //atributos do modelo de notificacao, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }
        [NotNull, MaxLength(100)]
        public TipoMensagem Motivo { get; set; }
        [MaxLength(255)]
        public string Texto { get; set; } = string.Empty;
        [NotNull]
        public DateTime DataEmissao { get; set; }
        public DateTime DataLeitura { get; set; }
        [NotNull]
        public bool Status { get; set; }
        [NotNull]
        public int UsuarioId { get; set; }
        public int MensagemId { get; set; }
    }
}
