using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Enums
{
    //enumeração para status de avaliação, para controle do processo de validação dos comentários
    public enum StatusAvaliacao
    {
        AnaliseAutomatica,
        AnaliseManual,
        Aprovada,
        Reprovada
    }
}
