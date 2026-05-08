using AppMaui.Core.Enums;
using SQLite;

namespace AppMaui.Core.Models
{
    public class Mensagem
    {
        //atributos do modelo de mensagem, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }
        [NotNull, MaxLength(20)]
        public TipoMensagem Tipo { get; set; }
        [NotNull, MaxLength(100)]
        public string Conteudo { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;
    }
}
