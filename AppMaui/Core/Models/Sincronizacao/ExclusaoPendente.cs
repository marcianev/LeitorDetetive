using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Models.Sincronizacao
{
    public class ExclusaoPendente
    {
        public string Tabela { get; set; } = string.Empty;
        public Guid Uuid { get; set; }
        public DateTime DataExclusao { get; set; }
    }
}
