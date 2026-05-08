using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.ViewsModels
{
    public partial class NovaSenhaViewModel : ObservableObject
    {
        public Action? OnFecharNovaSenha { get; set; }

        [RelayCommand]
        private void FecharNovaSenha()
        {
            OnFecharNovaSenha?.Invoke();
        }
    }
}
