using SQLite;


namespace AppMaui.Core.Models
{
    public class Aluno
    {
        //atributos do modelo de aluno, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }
        [MaxLength(100), NotNull]
        public string Nome { get; set; } = string.Empty;
        [NotNull]
        public string Nickname { get; set; } = string.Empty;
        public string CodigoAcesso { get; set; } = string.Empty;
        [NotNull]
        public int UsuarioId { get; set; }
        public int TurmaId { get; set; }
        public int PatenteId { get; set; }
    }
}
