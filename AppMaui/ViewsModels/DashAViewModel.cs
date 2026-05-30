using AppMaui.Core.DTOs;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using AppMaui.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;


namespace AppMaui.ViewsModels
{
    public partial class DashAViewModel : ObservableObject, 
        IRecipient<IniciarLeituraMessage>, IRecipient<ConcluirDesafio>,
        IRecipient<EnviarComentario>
    {
        [ObservableProperty]
        private DashBoardADTO? dash;
       
        private readonly AlunoService _alunoService;

        public DashAViewModel(AlunoService alunoService)
        {
            _alunoService = alunoService;
            WeakReferenceMessenger.Default.RegisterAll(this);
            _ = CarregarDashBoard();
        }

        public async Task CarregarDashBoard()
        {
            Dash = new DashBoardADTO();
            
            var usuario = SessaoService.UsuarioLogado;
            
            if (usuario != null && usuario.Tipo == "Aluno")
            {                
                Dash = await _alunoService.GerarDashBoard(usuario.Id);               
            }
                
        }

        public async void Receive(IniciarLeituraMessage message)
        {
            await CarregarDashBoard();
        }

        public async void Receive(ConcluirDesafio message)
        {
            await CarregarDashBoard();
        }

        public async void Receive(EnviarComentario message)
        {
            await CarregarDashBoard();
        }
    }
}
