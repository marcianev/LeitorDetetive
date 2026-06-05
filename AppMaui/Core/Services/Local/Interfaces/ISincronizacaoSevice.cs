using Shared.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Local.Interfaces
{
    public interface ISincronizacaoService
    {
        Task<OperacaoResponse> SincronizarEventos();
        Task<OperacaoResponse> SincronizarTudo();
    }
}
