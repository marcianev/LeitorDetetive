using SQLite;


namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa a resposta de um aluno para um desafio específico.
    /// </summary>
    public class Resposta
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        public string Palavra { get; set; } = string.Empty;

        [NotNull]
        public int AlunoId { get; set; }

        [NotNull]
        public int DesafioId { get; set; }

        [NotNull]
        public DateTime UltimoRegistro { get; set; }
    }
}
