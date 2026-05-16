using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.ViewsModels
{
    public partial class TurmaPViewModel : ObservableObject
    {
        [ObservableProperty]
        private string mensagem;
        [ObservableProperty]
        private bool mostrarCadastro;
        [ObservableProperty]
        private ObservableCollection<Turma> turmas = new();

        public Usuario _usuario;
        public CadastroTViewModel CadastroTVM { get; }
        private readonly TurmaService _turmaService;

        public TurmaPViewModel(CadastroTViewModel cadastroTVM, TurmaService turmaService)
        {
            CadastroTVM = cadastroTVM;
            _turmaService = turmaService;
            _usuario = new Usuario();
            Logado();
            CadastroTVM.OnFecharCadastro = () =>
            {
                MostrarCadastro = false;
                _ = CarregarTurmas();
            };
            CarregarTurmas();
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

        //listar turmas
        [RelayCommand]
        public async Task CarregarTurmas()
        {
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
