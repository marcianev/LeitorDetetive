using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa uma turma com alunos, professores e trilha de aprendizado associada.
    /// </summary>
    public class Turma
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [NotNull]
        public DateTime DataCriacao { get; set; }

        [MaxLength(4)]
        public string Ano { get; set; } = string.Empty;

        public int TamanhoTrilha { get; set; }

        [NotNull]
        public bool? Status { get; set; }

        [NotNull]
        public string CodigoAcesso { get; set; } = string.Empty;

        [NotNull]
        public int ProfessorId { get; set; }

        public int TrilhaId { get; set; }
    }
}
