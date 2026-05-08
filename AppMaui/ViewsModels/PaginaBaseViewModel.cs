using AppMaui.Core.Models;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.ViewsModels
{
    public partial class PaginaBaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string icones;
        private readonly OpenAIService _openAIService;
        private readonly CriptogramaService _criptogramaService;
        public ObservableCollection<LetraCriptografada> Letras { get; set; }

        public PaginaBaseViewModel(OpenAIService openAIService, CriptogramaService criptogramaService)
        {
            _openAIService = openAIService;
            _criptogramaService = criptogramaService;           
        }

        public void TestarCriptografia()
        {
            var lista = _criptogramaService.Gerar("TESTE");
            Letras = new ObservableCollection<LetraCriptografada>(lista);
            string mensagem = string.Join(
            " ",
            Letras.Select(x => x.Simbolo));

            Icones = mensagem;
        }

        [RelayCommand]
        private async Task ValidacaoAutomatica()
        {
           
                string comentario = "Oi Marta, vc é muita burra, não fale mais comigo.";
                string resposta = await _openAIService.ValidarComentario(comentario);

                if (string.IsNullOrWhiteSpace(resposta))
                    resposta = "Sem resposta";

                await Shell.Current.DisplayAlert("Validação do comentário: ", resposta, "OK");
                TestarCriptografia();

                    
        }


    }
}
