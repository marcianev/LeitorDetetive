using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Services.Interfaces
{
    public interface IDialogoService
    {
        Task<bool> Confirmar(string titulo, string mensagem, string aceitar, string cancelar);
        Task Mensagem(string titulo, string mensagem, string aceitar);
        Task<string> Consulta3(string titulo, string cancelar, string recusar, string aceitar);
    }
}
