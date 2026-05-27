using AppMaui.Core.Enums;
using SQLite;

namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa a avaliação de um livro feita por um usuário.
    /// Gerencia nota, comentário e status do processo de validação.
    /// </summary>
    public class Avaliacao
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull]
        public int Nota { get; set; }

        [MaxLength(300)]
        public string Comentario { get; set; } = string.Empty;
       
        // Status de validação do comentário (análise automática, manual, aprovada ou reprovada).        
        public StatusAvaliacao Status { get; set; }

        [NotNull]
        public int UsuarioId { get; set; }

        [NotNull]
        public int LivroId { get; set; }

        [NotNull]
        public DateTime DataCadastro { get; set; }
    }
}
