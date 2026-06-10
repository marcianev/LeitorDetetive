using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using AppMaui.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace AppMaui.ViewsModels
{
    public partial class AlunoPViewModel : ObservableObject,
        IRecipient<AdicionarTurma>
    {
        [ObservableProperty]
        private string? mensagem = string.Empty;
        [ObservableProperty]
        private bool mostrarCadastro;
        [ObservableProperty]
        private ObservableCollection<AlunoPDTO> alunos = [];
        [ObservableProperty]
        private ObservableCollection<Turma> turmas = [];
        [ObservableProperty]
        private Turma? turmaSelecionada;

        public CadastroAViewModel CadastroAVM { get; }
        private readonly AlunoService _alunoService;
        private readonly TurmaService _turmaService;

        public AlunoPViewModel(CadastroAViewModel cadastroAVM,
            AlunoService alunoService, TurmaService turmaService)
        {
            CadastroAVM = cadastroAVM;
            _alunoService = alunoService;
            _turmaService = turmaService;
            WeakReferenceMessenger.Default.Register(this);
            CadastroAVM.OnFecharCadastro = () =>
            {
                MostrarCadastro = false;
                _ = CarregaAlunos();
            };
            _ = CarregaAlunos();
            _ = CarregaTurmas();
        }

        //listar alunos
        [RelayCommand]
        public async Task CarregaAlunos()
        {
            if (TurmaSelecionada == null)
            {
                Mensagem = "Selecione uma turma";
            }
            else
            {
                Mensagem = string.Empty;
                var lista = await _alunoService.BuscarDadosAlunoP(TurmaSelecionada.Id);

                if (lista != null)
                    Alunos = new ObservableCollection<AlunoPDTO>(lista);
            }
        }

        //listar turmas
        [RelayCommand]
        public async Task CarregaTurmas()
        {
            var usuario = SessaoService.UsuarioLogado;

            if (usuario == null)
                return;
            if (usuario.Tipo == "Professor")
            {
                var lista = await _turmaService.ListarTurmaPorUsuario(usuario.Id);

                if (!lista.Sucesso)
                {
                    Mensagem = lista.Mensagem;
                    return;
                }

                Turmas = new ObservableCollection<Turma>(
                    lista.Dados ?? []);
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
        async partial void OnTurmaSelecionadaChanged(Turma? value)
        {            
            if (value == null)
                return;
            var usuario = SessaoService.UsuarioLogado;
            if (usuario == null)
                return;
            if (usuario.Tipo != "Professor")
                return;            
            await CarregaAlunos();

        }

        public async void Receive(AdicionarTurma adicionarTurma)
        {
            await CarregaTurmas();
        }
    }
}
