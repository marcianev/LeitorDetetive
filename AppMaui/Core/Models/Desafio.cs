using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um desafio interativo associado a um livro.
    /// Suporta diferentes tipos de desafios com pergunta, resposta e validação.
    /// </summary>
    public class Desafio
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string Pergunta { get; set; } = string.Empty;

        [NotNull, MaxLength(20)]
        public string Resposta { get; set; } = string.Empty;

        [NotNull]
        public int LivroId { get; set; }

        [NotNull]
        public string TipoDesafio { get; set; } = string.Empty;

        ///summary>Índices das letras que formam a palavra secreta (desafios do tipo palavra secreta).</summary>
        public int IndicesPalavraSecreta { get; set; }
    }
}
