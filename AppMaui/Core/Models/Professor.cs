using SQLite;


namespace AppMaui.Core.Models
{
    public class Professor
    {
        //atributos do modelo de aluno, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }
        [NotNull, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;
        [NotNull, MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [NotNull, MaxLength(15)]
        public string Cpf { get; set; } = string.Empty;
        [NotNull]
        public int UsuarioId { get; set; }
    }
}
