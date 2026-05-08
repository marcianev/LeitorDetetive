using SQLite;


namespace AppMaui.Core.Models
{
    public class Desafio
    {
        //atributos do modelo de desafio, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }
        [NotNull, MaxLength(100)]
        public string Pergunta { get; set; } = string.Empty;
        [NotNull, MaxLength(20)]
        public string Resposta { get; set; } = string.Empty;
        [NotNull]
        public int LivroId { get; set; }
    }
}
