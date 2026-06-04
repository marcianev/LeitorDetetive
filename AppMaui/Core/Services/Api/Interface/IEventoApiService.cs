using AppMaui.Core.Models;
using Shared.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.Api.Interface
{
    public interface IEventoApiService
    {
        Task<OperacaoResponse?> SalvarEvento(EventoSistema evento);
    }
}
