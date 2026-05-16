using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CadastroAViewModel CadastroAVM { get; }
        private readonly AlunoService _alunoService;

        public AlunoPViewModel(CadastroAViewModel cadastroAVM, AlunoService alunoService)
        {
            CadastroAVM = cadastroAVM;
            _alunoService = alunoService;
            CadastroAVM.OnFecharCadastro = () =>
            {
                MostrarCadastro = false;
                _ = CarregaAlunos();
            };
            CarregaAlunos();
        }

        //listar alunos
        [RelayCommand]
        public async Task CarregaAlunos()
        {            
            var lista = await _alunoService.ListarAlunos(1);
            if(lista != null) 
                Alunos = new ObservableCollection<Aluno>(lista);           
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
    }
}
