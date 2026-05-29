using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;


namespace AppMaui.ViewsModels
{
    public partial class DashPViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<DashBoardPDTO> dashBoards = [];
        private readonly AlunoService _alunoService;

        public Usuario _usuario;

        public DashPViewModel(AlunoService alunoService)
        {
            _alunoService = alunoService;
            _usuario = new();
            _ = Logado();
        }
        //receber dados do usuario logado
        public async Task Logado()
        {
            if (SessaoService.UsuarioLogado == null)
            {
               await Shell.Current.GoToAsync("Login");
                return;
            }
            _usuario = SessaoService.UsuarioLogado;
            await CarregarDados();
        }

        //metodo de carregamento
        public async Task CarregarDados()
        {          
            if (_usuario.Tipo != "Professor")
                return;

            var lista = await _alunoService.GerarDashBoardProfessor(_usuario.Id);
            if (lista != null)
                DashBoards = new ObservableCollection<DashBoardPDTO>(lista);    
        }
    }
}
