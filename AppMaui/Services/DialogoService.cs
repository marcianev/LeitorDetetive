using AppMaui.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Services
{
    public class DialogoService : IDialogoService
    {
        public async Task<bool> Confirmar(string titulo, string mensagem, string aceitar, string cancelar)
        {
           return await Shell.Current.CurrentPage.DisplayAlert(titulo, mensagem, aceitar, cancelar);
        }

        public async Task Mensagem(string titulo, string mensagem, string aceitar)
        {
            await Shell.Current.CurrentPage.DisplayAlert(titulo, mensagem, aceitar);
        }

        public async Task<string> Consulta3(string titulo,string cancelar, string recusar, string aceitar)
        {
            return await Shell.Current.CurrentPage.DisplayActionSheet(titulo, cancelar, recusar, aceitar);
        }
    }
}
