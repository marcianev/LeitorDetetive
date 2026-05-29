using AppMaui.Core.DTOs;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;


namespace AppMaui.ViewsModels
{
    public partial class DashAViewModel : ObservableObject
    {
        [ObservableProperty]
        private DashBoardADTO? dash;
       
        private readonly AlunoService _alunoService;

        public DashAViewModel(AlunoService alunoService)
        {
            _alunoService = alunoService;
            _ = CarregarDashBoard();
        }

        public async Task CarregarDashBoard()
        {
            
            var usuario = SessaoService.UsuarioLogado;
            
            if (usuario != null && usuario.Tipo == "Aluno")
            {                
                Dash = await _alunoService.GerarDashBoard(usuario.Id);               
            }
                
        }
    }
}
