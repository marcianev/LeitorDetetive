using AppMaui.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Services
{
    public class NavigationService : INavigationService
    {
        public async Task NavegarPara(string rota)
        {
            await Shell.Current.GoToAsync(rota);
        }
    }
}
