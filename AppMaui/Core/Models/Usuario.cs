using SQLite;

namespace AppMaui.Core.Models
{
    public class Usuario
    {
        //atributos do modelo de aluno, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }
        [NotNull, MaxLength(50), Unique]
        public string User { get; set; } = string.Empty;
        [NotNull]
        public string Senha { get; set; } = string.Empty;
        [NotNull]
        public bool? StatusSenha { get; set; }
        [NotNull]
        public bool? StatusUsuario { get; set; }
        [NotNull]
        public DateTime DataCadastro { get; set; }
        [NotNull]
        public string Tipo { get; set; } = string.Empty;
    }
}
