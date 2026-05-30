using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using AppMaui.Messages;
using AppMaui.Services.Interfaces;
using AppMaui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AppMaui.ViewsModels
{
    public partial class PaginaBaseViewModel : ObservableObject,
        IRecipient<AtivarLoading>
    {
        
        [ObservableProperty]
        private string? user;
        [ObservableProperty]
        private View? viewAtiva;
        [ObservableProperty]
        private bool botaoEdicao;
        [ObservableProperty]
        private bool modoEdicao;
        public Usuario _usuario;
        [ObservableProperty]
        private bool mostrarCadastro;
        [ObservableProperty]
        private string imgBotao1 = string.Empty;
        [ObservableProperty]
        private string imgBotao2 = string.Empty;
        [ObservableProperty]
        private string imgBotao3 = string.Empty;
        [ObservableProperty]
        private string lbBotao1 = string.Empty;
        [ObservableProperty]
        private string lbBotao2 = string.Empty;
        [ObservableProperty]
        private string lbBotao3 = string.Empty;
        [ObservableProperty]
        public bool carregando;    
        

        public CadastroPViewModel CadastroPVM { get; }

        private readonly DashPView  _dashPView;
        private readonly DashAView _dashAView;
        private readonly AlunoPView _alunoPView;
        private readonly EstanteAView _estanteAView;
        private readonly TurmaPView _turmaPView;
        private readonly DesafioView _desafio;
        private readonly AvaliacaoAView _avaliacaoAView;
        private Professor? _professor;
        private readonly ProfessorService _professorService;
        private readonly INavigationService _navigationService;
        private readonly IDialogoService _dialogoService;

        public PaginaBaseViewModel(DashPView dashPView, AlunoPView alunoPView, 
            TurmaPView turmaPView, CadastroPViewModel cadastroPVM, 
            ProfessorService professorService, INavigationService navigationService, 
            IDialogoService dialogoService, DashAView dashAView,
            EstanteAView estanteAView, DesafioView desafio, 
            AvaliacaoAView avaliacaoAView)
        {
            _dashPView = dashPView;
            _dashAView = dashAView;
            _alunoPView = alunoPView;
            _estanteAView = estanteAView;
            _turmaPView = turmaPView;
            _desafio = desafio;
            _avaliacaoAView = avaliacaoAView;           
            CadastroPVM = cadastroPVM;
            _professorService = professorService;
            _navigationService = navigationService;
            _dialogoService = dialogoService;
            _usuario = new Usuario();            
            _professor = new();           
            WeakReferenceMessenger.Default.Register(this);
        }

        public async void Inicializar()
        {
            try
            {                
                Carregando = true;
                await Task.Delay(1000);
                await Logado();
                CadastroPVM.OnFecharCadastro = () => MostrarCadastro = false;
                if (_usuario.Tipo == "Professor")
                {
                    ImgBotao1 = "alunos.webp";
                    LbBotao1 = "Alunos";
                    ImgBotao2 = "turmas.webp";
                    LbBotao2 = "Turmas";
                    ImgBotao3 = "comentarios.webp";
                    LbBotao3 = "Comentários";
                    BotaoEdicao = true;
                }
                if (_usuario.Tipo == "Aluno")
                {
                    ImgBotao1 = "livros.webp";
                    LbBotao1 = "Estante";
                    ImgBotao2 = "trofeu.webp";
                    LbBotao2 = "Desafios";
                    ImgBotao3 = "comentarios.webp";
                    LbBotao3 = "Comentários";
                    BotaoEdicao = false;
                }

                ModoEdicao = false;

                    await AbrirHome();
            }
            finally
            {
                Carregando = false;
            }
        }

        //metodo carrega label com nome do usuario e armazena o id de usuario
        public async Task Logado()
        {           
            if (SessaoService.UsuarioLogado == null)
            {
                await _navigationService.NavegarPara("///Login");
                return;
            }
            _usuario = SessaoService.UsuarioLogado;
            User = _usuario.User;
        }

        //abre a view Principal no body
        [RelayCommand]
        private async Task AbrirHome()
        {
            if(_usuario == null)
            {
                await _dialogoService.Confirmar("Erro", "Usuário não encontrado. Faça login novamente.", "OK", "SAIR");
                await _navigationService.NavegarPara("///Login");
                return;
            }
            if (_usuario.Tipo == "Professor")
                ViewAtiva = _dashPView;
            if(_usuario.Tipo == "Aluno")
                ViewAtiva = _dashAView;
        }

        //abre a view do primeiro botão
        [RelayCommand]
        private async Task AbrirPrimView()
        {
            if (_usuario == null)
            {
                await _dialogoService.Confirmar("Erro", "Usuário não encontrado. Faça login novamente.", "OK", "SAIR");
                await _navigationService.NavegarPara("///Login");
                return;
            }
            if (_usuario.Tipo == "Professor")
                ViewAtiva = _alunoPView;
            if(_usuario.Tipo == "Aluno")
                ViewAtiva = _estanteAView;           
        }

        //abre a view do segundo botão
        [RelayCommand]
        private async Task AbrirSegView()
        {
            if (_usuario == null)
            {
                await _dialogoService.Confirmar("Erro", "Usuário não encontrado. Faça login novamente.", "OK", "SAIR");
                await _navigationService.NavegarPara("///Login");
                return;
            }
            if (_usuario.Tipo == "Professor")
                ViewAtiva = _turmaPView;
            if (_usuario.Tipo == "Aluno")
                ViewAtiva = _desafio;
        }

        //abre a view do terceiro botão
        [RelayCommand]
        private async Task AbrirTercView()
        {
            if (_usuario == null)
            {
                await _dialogoService.Confirmar("Erro", "Usuário não encontrado. Faça login novamente.", "OK", "SAIR");
                await _navigationService.NavegarPara("///Login");
                return;
            }            
                ViewAtiva = _avaliacaoAView;
        }

        //mostrar view de edição de usuário
        [RelayCommand]
        private async Task AbrirEdicaoUser()
        {
            if (_usuario.Tipo == "Professor")
            {
                _professor = await _professorService.BuscarProfessorPorUsuario(_usuario.Id);
                if (_professor == null)
                    throw new Exception("Professor não encontrado.");
                else
                {
                    CadastroPVM.Id = _professor.Id;
                    CadastroPVM.User = _usuario.User;
                    CadastroPVM.UserTemp = _usuario.User;
                    CadastroPVM.Nome = _professor.Nome;
                    CadastroPVM.Email = _professor.Email;
                    CadastroPVM.Cpf = _professor.Cpf;
                    CadastroPVM.UsuarioId = _usuario.Id;
                    CadastroPVM.ModoAlterar = true;
                    MostrarCadastro = true;
                }
            }  
        }

        //realizar o logout
        [RelayCommand]
        private async Task Sair()
        {
            
            if(_usuario != null)
            {                
                SessaoService.Logout();
            }
            await _navigationService.NavegarPara("///Login");
        }      
        
        public async void Receive(AtivarLoading message)
        {
            await Shell.Current.DisplayAlert(
        "Loading",
        $"{message.Value}\n{Environment.StackTrace}",
        "OK");

            Carregando = message.Value;
        }
    }
}
