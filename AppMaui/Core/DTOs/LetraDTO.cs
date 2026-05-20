using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.DTOs
{
    public partial class LetraDTO : ObservableObject
    {
        [ObservableProperty]
        private string letraDigitada = string.Empty;
        [ObservableProperty]
        private bool correta;
        public string LetraOriginal { get; set; } = string.Empty;
        public string LetraCriptograma { get; set; } = string.Empty;
        public int Posicao { get; set; }
        public bool Destacada { get; set; }

        partial void OnLetraDigitadaChanged(string value)
        {
            Correta = value.ToUpper() == LetraOriginal.ToUpper();
        }
    }
}
