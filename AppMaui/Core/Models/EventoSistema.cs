using Shared.Enums;
using SQLite;
using System.ComponentModel.DataAnnotations;


namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um registro de eventos com fins de notificações, dashboards e exclusões.
    /// </summary>
    public class EventoSistema
    {
        [PrimaryKey, AutoIncrement, Required, Unique]
        public int Id { get; set; }

        [Required]
        public Guid Identificador { get; set; }

        [Required]
        public string Tabela { get; set; } = string.Empty;

        [Required]
        public EventosEnum TipoEvento { get; set; }

        [Required]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public DateTime DataEvento{ get; set; }

        public int? ReferenciaId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        public bool Sincronizado { get; set; }

        public DateTime? DataSincronizado { get; set; }
    }
}
