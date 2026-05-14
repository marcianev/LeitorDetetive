using AppMaui.Core.Services.Application;
using AppMaui.Core.Services.Interfaces;
using AppMaui.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppMaui.ViewsModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? usuario;

        [ObservableProperty]
        private string? senha;

        [ObservableProperty]
        private string? mensagem;

        [ObservableProperty]
        private bool mostrarCadastro;

        [ObservableProperty]
        private bool mostrarNovaSenha;

        [ObservableProperty]
        private bool mostrarPrimeiroAcesso;

        public CadastroPViewModel CadastroPVM { get; }

        public NovaSenhaViewModel NovaSenhaVM { get; }

        public PAcessoViewModel PrimeiroAcessoVM { get; }

        private readonly IAutenticacaoService _auth;

        private readonly INavigationService _navigationService;


        public LoginViewModel(IAutenticacaoService auth,
                INavigationService nav,
                IValidationService ivs,
                CadastrarProfessorService cps,
                CadastroPViewModel cadastroPVM,
                NovaSenhaViewModel novaSenhaVM,
                PAcessoViewModel pAcessoVM)
        {
            CadastroPVM = cadastroPVM;
            NovaSenhaVM = novaSenhaVM;
            PrimeiroAcessoVM = pAcessoVM;
            _auth = auth;
            _navigationService = nav;
                      
            CadastroPVM.OnFecharCadastro = () => MostrarCadastro = false;
            NovaSenhaVM.OnFecharNovaSenha = () => MostrarNovaSenha = false;
            PrimeiroAcessoVM.OnFecharPAcesso = () => MostrarPrimeiroAcesso = false;
        }

        //mostra a view de cadastro
        [RelayCommand]
        private void AbrirCadastro()
        {           
            MostrarCadastro = true;
        }

        //mostra a view para pedir nova senha
        [RelayCommand]
        private void AbrirNovaSenha()
        {
            MostrarNovaSenha = true;
        }

        //mostra a view para primeiro acesso
        [RelayCommand]
        private void AbrirPrimeiroAcesso()
        {
            MostrarPrimeiroAcesso = true;
        }

        //chama o processo de login
        [RelayCommand]
        private async Task Entrar()
        {           
            if(string.IsNullOrWhiteSpace(Usuario)||
                string.IsNullOrWhiteSpace(Senha))
            {
                Mensagem = "Informe usuário e senha.";
                await Task.Delay(3000);
                Mensagem = "";
                return;
            }
            var user = await _auth.Autenticar(Usuario, Senha);

            if(user != null)
            {
                
                await _navigationService.NavegarPara("PaginaBase");
            }
            else
            {
                Mensagem = "Usuário ou senha inválido.";
                await Task.Delay(3000);
                Mensagem = "";
                return;
            }

           
        }
    }
}
