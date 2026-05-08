using AppMaui.Core.Services.External;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.ViewsModels
{
    public partial class PaginaBaseViewModel : ObservableObject
    {
        private readonly OpenAIService _openAIService;

        public PaginaBaseViewModel(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [RelayCommand]
        private async Task ValidacaoAutomatica()
        {
           
                string comentario = "Oi Marta, vc é muita burra, não fale mais comigo.";
                string resposta = await _openAIService.ValidarComentario(comentario);

                if (string.IsNullOrWhiteSpace(resposta))
                    resposta = "Sem resposta";

                await Shell.Current.DisplayAlert("Validação do comentário: ", resposta, "OK");

                    
        }
    }
}
