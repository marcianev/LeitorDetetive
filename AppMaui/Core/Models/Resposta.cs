using SQLite;


namespace AppMaui.Core.Models
{
    public class Resposta
    {
        //atributos do modelo de aluno, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }
        public string Palavra { get; set; } = string.Empty;//recebe resposta
        [NotNull]
        public int AlunoId { get; set; }
        [NotNull]
        public int DesafioId { get; set; }
        [NotNull]
        public DateTime UltimoRegistro { get; set; }
    }
}
