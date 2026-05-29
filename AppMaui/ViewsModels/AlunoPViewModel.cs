using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AppMaui.ViewsModels
{
    public partial class AlunoPViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? mensagem;
        [ObservableProperty]
        private bool mostrarCadastro;
        [ObservableProperty]
        private ObservableCollection<Aluno> alunos = new();
        [ObservableProperty]
        private ObservableCollection<Turma> turmas = new();
        [ObservableProperty]
        private Turma turmaSelecionada;

        public CadastroAViewModel CadastroAVM { get; }
        private readonly AlunoService _alunoService;
        private readonly TurmaService _turmaService;

        public AlunoPViewModel(CadastroAViewModel cadastroAVM, 
            AlunoService alunoService, TurmaService turmaService)
        {
            CadastroAVM = cadastroAVM;
            _alunoService = alunoService;
            _turmaService = turmaService;
            CadastroAVM.OnFecharCadastro = () =>
            {
                MostrarCadastro = false;
                _ = CarregaAlunos();
            };
            CarregaAlunos();
            CarregaTurmas();
        }

        //listar alunos
        [RelayCommand]
        public async Task CarregaAlunos()
        {          
            if(TurmaSelecionada == null)
            {
                Mensagem = "Selecione uma turma";

            }
            else
            {
                Mensagem = string.Empty;
                var lista = await _alunoService.ListarAlunos(turmaSelecionada.Id);
                if (lista != null)
                    Alunos = new ObservableCollection<Aluno>(lista);
            }
                    
        }

        //listar turmas
        [RelayCommand]
        public async Task CarregaTurmas()
        {
            var usuario = SessaoService.UsuarioLogado;
            if(usuario.Tipo == "Professor")
            {
                  var lista = await _turmaService.ListarTurmaPorUsuario(usuario.Id);

            if (lista != null)
                Turmas = new ObservableCollection<Turma>(lista);
            }
            
          
        }

        //abrir overlay de cadastro
        [RelayCommand]
        private void AbrirCadastro()
        {
            CadastroAVM.ModoAlterar = false;
            MostrarCadastro = true;
        }

        //abrir overlay para alterações
        [RelayCommand]
        private void AlterarCadastro(Aluno aluno)
        {     
            CadastroAVM.ModoAlterar = true;
            CadastroAVM.Nome = aluno.Nome;
            CadastroAVM.Aluno = aluno;            
            MostrarCadastro = true;
        }

        //carregad os alunos da turma selecionada
        async partial void OnTurmaSelecionadaChanged(Turma value)
        {
            if (value == null)
                return;
            var usuario = SessaoService.UsuarioLogado;
            if (usuario.Tipo != "Professor")
                return;
            await CarregaAlunos();
           
        }
    }
}
