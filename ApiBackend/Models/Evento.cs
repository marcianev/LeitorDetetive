using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ApiBackend.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid Identificador { get; set; }

        [Required]
        public string Tabela { get; set; } = string.Empty;

        [Required, MaxLength(10)]
        public EventosEnum TipoEvento { get; set; }

        [Required, MaxLength(50)]
        public string Descricao { get; set; } = string.Empty;
       
        [Column(TypeName = "timestamp without time zone")]
        public DateTime DataEvento { get; set; }
      
        public int? ReferenciaId { get; set; }

        [Required]
        public int UsuarioId { get; set; }      
    }
}
