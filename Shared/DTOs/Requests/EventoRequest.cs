using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Enums;

namespace Shared.DTOs.Requests
{
    public class EventoRequest
    {
        [Required]
        public string Tabela { get; set; } = string.Empty;
       
        public EventosEnum TipoEvento { get; set; }

        [Required, MaxLength(50)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public DateTime DataEvento { get; set; }

        public int? ReferenciaId { get; set; }

        [Required]
        public int UsuarioId { get; set; }
    }
}
