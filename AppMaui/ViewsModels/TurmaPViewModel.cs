using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AppMaui.ViewsModels
{
    public partial class TurmaPViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? mensagem;
        [ObservableProperty]
        private bool mostrarCadastro;
        [ObservableProperty]
        private ObservableCollection<Turma> turmas = new();
        [ObservableProperty]
        private ObservableCollection<LivrosMaisLidosDTO> topLivros = new();
        [ObservableProperty]
        private ObservableCollection<LivrosBemAvaliadosDTO> bemAvaliados = new();

        public Usuario _usuario;
        public CadastroTViewModel CadastroTVM { get; }
        private readonly TurmaService _turmaService;
        private readonly LeituraService _leituraService;
        private readonly AvaliacaoService _avaliacaoService;


        public TurmaPViewModel(CadastroTViewModel cadastroTVM, TurmaService turmaService,
            LeituraService leituraService, AvaliacaoService avaliacaoService)
        {
            CadastroTVM = cadastroTVM;
            _turmaService = turmaService;
            _leituraService = leituraService;
            _avaliacaoService = avaliacaoService;
            _usuario = new Usuario();
            Logado();
            CadastroTVM.OnFecharCadastro = () =>
            {
                MostrarCadastro = false;
                _ = CarregarTurmas();
                _ = CarregarRanking();    
                _ = CarregarBemAvaliados();
            };
            _ = CarregarTurmas();
            _ = CarregarRanking();
            _ = CarregarBemAvaliados();
        }

        //receber dados do usuario logado
        public void Logado()
        {
            if(SessaoService.UsuarioLogado == null)
            {
                Shell.Current.GoToAsync("Login");
                return;
            }
            _usuario = SessaoService.UsuarioLogado;            
        }

        //carregar os mais bem avaliados
        public async Task CarregarBemAvaliados()
        {
            if(_usuario.Tipo != "Professor")
                return;

            var lista = await _avaliacaoService.TopAvaliados(_usuario.Id);
            if(lista != null)
                BemAvaliados = new ObservableCollection<LivrosBemAvaliadosDTO>(lista);
        }

        //carregar os mais lidos
        public async Task CarregarRanking()
        {
            if (_usuario.Tipo != "Professor")
                return;

            var lista = await _leituraService.RankingLeitura(_usuario.Id);  
            if(lista != null)
                TopLivros = new ObservableCollection<LivrosMaisLidosDTO>(lista);            
        }

        //listar turmas
        [RelayCommand]
        public async Task CarregarTurmas()
        {
            if (_usuario.Tipo != "Professor")
                return;
            var lista = await _turmaService.ListarTurmaPorUsuario(_usuario.Id);
            if (lista != null)
                Turmas = new ObservableCollection<Turma>(lista);
        }

        //abrir overlay de cadastro
        [RelayCommand]
        private void AbrirCadastro()
        {
            CadastroTVM.ModoAlterar = false;
            MostrarCadastro = true;
        }

        //abrir overlay para alterações
        [RelayCommand]
        private void AlterarCadastro(Turma turma)
        {
            CadastroTVM.Turma = turma;
            CadastroTVM.Nome = turma.Nome;
            CadastroTVM.Turma = turma;            
            MostrarCadastro = true;
        }       
    }
}
