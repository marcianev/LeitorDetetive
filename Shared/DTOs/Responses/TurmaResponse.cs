using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Responses
{
    public class TurmaResponse
    {
        public Guid Uuid { get; set; } 
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } 
        public string Ano { get; set; } = string.Empty;
        public int TamanhoTrilha { get; set; }
        public bool? Status { get; set; } 
        public string? CodigoAcesso { get; set; } = string.Empty;
        public int ProfessorId { get; set; }
        public int TrilhaId { get; set; }
        public DateTime? DataAlterado { get; set; }
    }
}
