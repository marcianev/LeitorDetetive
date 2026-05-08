using SQLite;

namespace AppMaui.Core.Models
{
    public class Patente
    {
        //atributos do modelo de aluno, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
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
