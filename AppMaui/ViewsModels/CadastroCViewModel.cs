using AppMaui.Core.DTOs;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppMaui.ViewsModels
{
    public partial class CadastroCViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? titulo;
        [ObservableProperty]
        private string? viewComentario;
        [ObservableProperty]
        private int viewLivroId;
        [ObservableProperty]
        private int viewUsuarioId;
        [ObservableProperty]
        private int viewNota;
        [ObservableProperty]
        private string? mensagem;

        private readonly AvaliacaoService _avaliacaoService;
        private AvaliacaoDTO _dto;

        public Action? OnFecharCadastro { get; set; }

        public CadastroCViewModel(AvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
            _dto = new();
        }

        //salvar comentário
        [RelayCommand]
        private async Task SalvarComentario()
        {
            if (string.IsNullOrWhiteSpace(ViewComentario) || 
                string.IsNullOrWhiteSpace(Titulo))
            {
                Mensagem = "Sem título.";
                await Task.Delay(3000);
                Mensagem = "";
                return;
            }
            if (ViewLivroId <= 0 || ViewUsuarioId <= 0 || ViewNota <= 0)
            {
                Mensagem = "Dados insufientes.";
                await Task.Delay(3000);
                Mensagem = "";
                return;
            }

            _dto = new()
            {
                Nota = ViewNota,
                Comentario = ViewComentario,
                UsuarioId = ViewUsuarioId,
                LivroId = ViewLivroId
            };
            var salvo = await _avaliacaoService.SalvarAvaliacao(_dto);
            if (salvo)
            {             
                Mensagem = "Vamos dar uma olhada na sua mensagem.";
                await Task.Delay(3000);
                Mensagem = "";
                FecharCadastro();
            }              
            else
                Mensagem = "Erro ao salvar a avaliação.";
        }

        //fechar view
        [RelayCommand]
        private void FecharCadastro()
        {
           
            Titulo = string.Empty;
            ViewComentario = string.Empty;            
            OnFecharCadastro?.Invoke();
        }

    }
}
