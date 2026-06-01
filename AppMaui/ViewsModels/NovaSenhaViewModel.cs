using AppMaui.Core.Services.Api.Interface;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Local;
using AppMaui.Core.Services.Security;
using AppMaui.Services;
using AppMaui.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs.Requests;

namespace AppMaui.ViewsModels
{
    public partial class NovaSenhaViewModel : ObservableObject
    {
        [ObservableProperty]
        private string user = string.Empty;
        [ObservableProperty]
        private string email = string.Empty;
        [ObservableProperty]
        private string mensagem = string.Empty;

        public Action? OnFecharNovaSenha { get; set; }       
      
        private readonly ConectividadeService _conectividadeService;
        private readonly IAuthApiService _authApiService;

        public NovaSenhaViewModel( ConectividadeService conectividadeService,
            IAuthApiService authApiService)
        {           
            _conectividadeService = conectividadeService;
            _authApiService = authApiService;
        }

        [RelayCommand]
        private void FecharNovaSenha()
        {
            OnFecharNovaSenha?.Invoke();
        }

        [RelayCommand]
        private async Task EnviarNovaSenha()
        {
            var temInternet = _conectividadeService.TemInternet();
            if (!temInternet)
            {
                Mensagem = "É necessário conexão para nova senha";
                await Task.Delay(3000);
                Mensagem = string.Empty;                
                return;
            }
            if (string.IsNullOrWhiteSpace(User) || string.IsNullOrWhiteSpace(Email))
            {
                Mensagem = "Usuário e Email são obrigatórios.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }
            
            var request = new RecuperarSenhaRequest
            {
                User = User,
                Email = Email
            };

            bool resultado = await _authApiService.RecuperarSenha(request);

            if (!resultado)
            {
                Mensagem = "Usuário ou email inválido.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }               

            Mensagem = "Código enviado ao seu email.";
            await Task.Delay(3000);
            Mensagem = string.Empty;
            FecharNovaSenha();      
        }
    }
  
}
