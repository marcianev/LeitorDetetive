using AppMaui.Core.Enums;
using SQLite;


namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa um registro de eventos com fins de notificações, dashboards e exclusões.
    /// </summary>
    public class EventoSistema
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull]
        public string Tabela { get; set; } = string.Empty;

        [NotNull, MaxLength(10)]
        public Eventos TipoEvento { get; set; }

        [NotNull, MaxLength(50)]
        public string Descricao { get; set; } = string.Empty;

        [NotNull]
        public DateTime DataEvento{ get; set; }

        [NotNull]
        public int ReferenciaId { get; set; }

        [NotNull]
        public int UsuarioId { get; set; }
    }
}
