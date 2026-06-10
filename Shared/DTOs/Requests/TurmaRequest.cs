using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Requests
{
    public class TurmaRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid Uuid { get; set; }

        [Required, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

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

        public DateTime? DataAlterado { get; set; }
    }
}
