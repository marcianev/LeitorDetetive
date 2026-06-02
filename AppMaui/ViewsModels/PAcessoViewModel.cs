using AppMaui.Core.DTOs;
using AppMaui.Core.Services.Api.Interface;
using AppMaui.Core.Services.Security;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs.Requests;
using Shared.DTOs.Responses;

namespace AppMaui.ViewsModels
{
    public partial class PAcessoViewModel : ObservableObject
    {
        //declarar variaveis locais
        [ObservableProperty]
        private string? user;
        [ObservableProperty]
        private string? senhaProvisoria;
        [ObservableProperty]
        private string? novaSenha;
        [ObservableProperty]
        private string? confirmacaoNovaSenha;
        [ObservableProperty]
        private string? mensagem;

        public Action? OnFecharPAcesso { get; set; }

        //injetar serviços necessários
        private readonly SenhaService _senhaService;       
        private readonly IAuthApiService _authApiService;
        

        public PAcessoViewModel(SenhaService senhaService, IAuthApiService authApiService)
        {

            _senhaService = senhaService;    
            _authApiService = authApiService;            
        }

        //comando para salvar nova senha
        [RelayCommand]
        public async Task SalvarNovaSenha()
        {
            if (string.IsNullOrEmpty(User) ||
            string.IsNullOrWhiteSpace(SenhaProvisoria) ||
            string.IsNullOrWhiteSpace(NovaSenha) ||
            string.IsNullOrWhiteSpace(ConfirmacaoNovaSenha))
            {
                Mensagem = "Todos os campos são obrigatórios.";
                await Task.Delay(3000);
                Mensagem = "";
                FecharPAcesso();
                return;
            }

            //confirmar senha iguais
            if (NovaSenha != ConfirmacaoNovaSenha)
            {
                Mensagem = "A nova senha e a confirmação não coincidem.";
                await Task.Delay(3000);
                Mensagem = "";
                FecharPAcesso();
                return;
            }


            //criar dto
            var request = new AcessoProvisorioRequest()
            {
                User = User,
                NovaSenha = NovaSenha,
                SenhaProvisoria = SenhaProvisoria
            };         

           var resultado = await _authApiService.AcessoProvisorio(request);

            if (resultado == null) 
            {
                Mensagem = "Erro de comunicação.";
                await Task.Delay(3000);
                Mensagem = "";
                FecharPAcesso();
            }
            else
            {
                Mensagem = resultado.Mensagem;
                await Task.Delay(3000);
                Mensagem = "";
                FecharPAcesso();
            }         
        }

        //metodo esconde view de acesso provisório
        [RelayCommand]
        private void FecharPAcesso()
        {
            User = string.Empty;
            SenhaProvisoria = string.Empty;
            NovaSenha = string.Empty;
            ConfirmacaoNovaSenha = string.Empty;
            OnFecharPAcesso?.Invoke();
        }
    }
    
}
