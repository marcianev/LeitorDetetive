using AppMaui.Core.Enums;
using SQLite;

namespace AppMaui.Core.Models
{
    public class Leitura
    {
        //atributos do modelo de leitura, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
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
