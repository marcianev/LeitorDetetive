using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ApiBackend.Models
{
    public class Turma
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid Uuid { get; set; }

        [Required, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;
        [Column(TypeName = "timestamp without time zone")]
        [Required]
        public DateTime DataCriacao { get; set; }

        [MaxLength(4)]
        public string Ano { get; set; } = string.Empty;

        public int TamanhoTrilha { get; set; }

        [Required]
        public bool? Status { get; set; }

        public string? CodigoAcesso { get; set; } = string.Empty;

        [Required]
        public int ProfessorId { get; set; }

        public int TrilhaId { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? DataAlterado { get; set; }
    }
}
