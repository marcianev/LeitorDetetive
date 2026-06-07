using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Api.Interface;
using AppMaui.Core.Services.Interfaces;
using AppMaui.Core.Services.Local;
using AppMaui.Core.Services.Local.Interfaces;
using AppMaui.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.Enums;
using System.Diagnostics;

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

        [ObservableProperty]
        private bool formulario;


        public CadastroPViewModel CadastroPVM { get; }

        public NovaSenhaViewModel NovaSenhaVM { get; }

        public PAcessoViewModel PrimeiroAcessoVM { get; }      

        private readonly INavigationService _navigationService;

        private readonly IAuthApiService _authApiService;

        private readonly ISincronizacaoService _sincronizacaoService; 

        private readonly SessaoService _sessaoService;

        private readonly EventoService _eventoService;

        public LoginViewModel(
                INavigationService nav,
                IValidationService ivs,                
                CadastroPViewModel cadastroPVM,
                NovaSenhaViewModel novaSenhaVM,
                PAcessoViewModel pAcessoVM,
                IAuthApiService authApiService,
                SessaoService sessaoService,
                EventoService eventoService,
                ISincronizacaoService sincronizacaoService)
        {
            CadastroPVM = cadastroPVM;
            NovaSenhaVM = novaSenhaVM;
            PrimeiroAcessoVM = pAcessoVM;    
            _authApiService = authApiService;
            _sessaoService = sessaoService;
            _eventoService = eventoService;
            _navigationService = nav;
            _sincronizacaoService = sincronizacaoService;
            CadastroPVM.OnFecharCadastro = () => MostrarCadastro = false;
            CadastroPVM.OnCarregando = (valor) => Carregando = valor;
            NovaSenhaVM.OnFecharNovaSenha = () => MostrarNovaSenha = false;
            PrimeiroAcessoVM.OnFecharPAcesso = () => MostrarPrimeiroAcesso = false;
            Formulario = false;
            EhSenha = true;
            Olho = "\uf06e";
            _ = Inicializar();
        }

        public async Task Inicializar()
        {
            Carregando = true;
            if (!await _sessaoService.ValidarToken())
            {                
                Formulario = true;
                Carregando = false;
                return;
            }                             
                await _sessaoService.RestaurarSessao();
                await _navigationService.NavegarPara("PaginaBase");  
         
            Carregando = false;
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
                Carregando = true;
                if (string.IsNullOrWhiteSpace(Usuario) ||
               string.IsNullOrWhiteSpace(Senha))
                {
                    Mensagem = "Informe usuário e senha.";
                    await Task.Delay(3000);
                    Mensagem = "";
                    return;
                }
                var user = await _authApiService.Login(Usuario, Senha);
                
                if (user != null && user.StatusSenha == true)
                {
                    await _sessaoService.LoginApi(user);
                    var evento = new EventoSistema
                    {
                        UsuarioId = user.UsuarioId,
                        TipoEvento = EventosEnum.Login,
                        Tabela = "Usuario",                       
                        Descricao = $"Usuário {user.User} realizou login com sucesso."
                    };

                    await _eventoService.SalvarEvento(evento);
                    await _sincronizacaoService.SincronizarTudo();
                    Usuario = string.Empty;
                    Senha = string.Empty;                    
                    await _navigationService.NavegarPara("PaginaBase");
                }
                else
                {
                    Mensagem = user.Mensagem;
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
