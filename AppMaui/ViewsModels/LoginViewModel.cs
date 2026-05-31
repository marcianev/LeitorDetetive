using AppMaui.Core.Services.Api.Interface;
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

        [ObservableProperty]
        private bool carregando;

        [ObservableProperty]
        private bool ehSenha;

        [ObservableProperty]
        private string olho;


        public CadastroPViewModel CadastroPVM { get; }

        public NovaSenhaViewModel NovaSenhaVM { get; }

        public PAcessoViewModel PrimeiroAcessoVM { get; }

        private readonly IAutenticacaoService _auth;

        private readonly INavigationService _navigationService;

        private readonly IAuthApiService _authApiService;


        public LoginViewModel(IAutenticacaoService auth,
                INavigationService nav,
                IValidationService ivs,
                CadastrarProfessorService cps,
                CadastroPViewModel cadastroPVM,
                NovaSenhaViewModel novaSenhaVM,
                PAcessoViewModel pAcessoVM,
                IAuthApiService authApiService)
        {
            CadastroPVM = cadastroPVM;
            NovaSenhaVM = novaSenhaVM;
            PrimeiroAcessoVM = pAcessoVM;            
            _auth = auth;
            _authApiService = authApiService;
            _navigationService = nav;                 
            CadastroPVM.OnFecharCadastro = () => MostrarCadastro = false;
            CadastroPVM.OnCarregando = (valor) => Carregando = valor;
            NovaSenhaVM.OnFecharNovaSenha = () => MostrarNovaSenha = false;
            PrimeiroAcessoVM.OnFecharPAcesso = () => MostrarPrimeiroAcesso = false;
            EhSenha = true;
            Olho = "\uf06e";
        }

        //mostra a view de cadastro
        [RelayCommand]
        private void AbrirCadastro()
        {
            CadastroPVM.ModoAlterar = false;
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
            try
            {
                var resultado = await _authApiService.Login(
                    "admin",
                    "123");

                if (resultado != null)
                {
                    await Shell.Current.DisplayAlert(
                        "Sucesso",
                        resultado.User,
                        "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert(
                        "Erro",
                        "Falha no login API",
                        "OK");
                }
                Carregando = true;
                if (string.IsNullOrWhiteSpace(Usuario) ||
               string.IsNullOrWhiteSpace(Senha))
                {
                    Mensagem = "Informe usuário e senha.";
                    await Task.Delay(3000);
                    Mensagem = "";
                    return;
                }
                var user = await _auth.Autenticar(Usuario, Senha);

                if (user != null && user.StatusSenha == true)
                {
                    Usuario = string.Empty;
                    Senha = string.Empty;
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
            finally
            {
                Carregando = false;
            }

           
        }

        [RelayCommand]
        private async Task VisualizarSenha()
        {
            EhSenha = !EhSenha;

            Olho = EhSenha ? "\uf06e" : "\uf070";
        }
    }
}
