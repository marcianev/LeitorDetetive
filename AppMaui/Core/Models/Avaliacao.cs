using AppMaui.Core.Enums;
using SQLite;


namespace AppMaui.Core.Models
{
    public class Avaliacao
    {
        //atributos do modelo de avaliação, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }
        [NotNull]
        public int Nota { get; set; }
        [MaxLength(100)]
        public string Comentario { get; set; } = string.Empty;
        [NotNull, MaxLength(20)]
        public StatusAvaliacao Status { get; set; }
        [NotNull]
        public int AlunoId { get; set; }
        [NotNull]
        public int LivroId { get; set; }
        [NotNull]
        public DateTime DataCadastro { get; set; }
    }
}
