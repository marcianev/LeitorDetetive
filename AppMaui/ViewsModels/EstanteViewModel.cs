using AppMaui.Core.DTOs;
using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using AppMaui.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;


namespace AppMaui.ViewsModels
{
    public partial class EstanteViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? titulo;
        [ObservableProperty]
        private string? capa;
        [ObservableProperty]
        private ObservableCollection<EstanteDTO> estanteDTOs = new();
        [ObservableProperty]
        private ImageSource? capaAtual;
        [ObservableProperty]
        private ImageSource? capaAnterior;
        [ObservableProperty]
        private bool livroAtual = false;
        [ObservableProperty]
        private bool proximoLivro = false;

        private readonly LivroService _livroService;
        private readonly LeituraService _leituraService;
        private readonly Leitura _leitura = new();
        private Livro? _livroSelecionado = new();
        private readonly IDialogoService _dialogoService;
        private readonly Usuario? _usuario = new();

        public EstanteViewModel(LivroService livroService,
            IDialogoService dialogoService, LeituraService leituraService)
        {
            _livroService = livroService;
            _dialogoService = dialogoService;
            _leituraService = leituraService;
            _usuario = SessaoService.UsuarioLogado;
            _ = CarregarLivros();
        }

        public async Task CarregarLivros()
        {
            //criar uma lista unica de consulta
            var lista = await _livroService.ListarLivros();
            if (lista != null)
            {
                foreach (var livro in lista)
                {
                    if (_usuario == null)
                        return;

                    var leitura = await _leituraService.GetLeituraPorLivroUsuario
                        (livro.Id, _usuario.Id);

                    if(leitura != null)
                    {     
                        if(leitura.Status == StatusLeitura.Concluida)
                            CapaAnterior = livro.Capa;
                        if(leitura.Status == StatusLeitura.Iniciada)
                            CapaAtual = livro.Capa;                        
                    }
                    EstanteDTOs.Add(new EstanteDTO
                    {
                        IdLivro = livro.Id,
                        Titulo = livro.Titulo,
                        Capa = livro.Capa,
                        IdLeitura = leitura?.Id ?? 0,
                        Status = leitura?.Status,
                        UsuarioId = leitura?.UsuarioId ?? 0,
                    });
                }
            }

            if(CapaAtual == null)
                AjustarVisibilidade(true);
            else
                AjustarVisibilidade(false);
        }

        //comando para escolher o livro e carregar a capa
        [RelayCommand]
        private async Task EscolherLivro(EstanteDTO livro)
        {
            
            if (livro.IdLivro <=0)
            {
                await _dialogoService.Mensagem("Escolher um livro.", "Selecione um livro antes de iniciar leitura.", "OK");
                return;
            }
            else
            {
                _livroSelecionado = await _livroService.BuscarLivroPorId(livro.IdLivro);
                Capa = livro.Capa;
            }            
        }

        //comando para iniciar a leitura do livro selecionado, verificando se já existe uma leitura registrada para o livro e usuário, se sim, verificar se a leitura está concluída, se estiver, exibir mensagem informando que a leitura já foi realizada, caso contrário, iniciar a leitura e salvar no banco de dados.
        [RelayCommand]
        private async Task IniciarLeitura()
        {
            if (_usuario == null)
                return;

            if (_livroSelecionado == null || _livroSelecionado.Id <= 0)
            {
                await _dialogoService.Mensagem("Escolhar um livro.", "Selecione um livro antes de iniciar leitura.", "OK");
                return;
            }            
            var primeiraLeitura = await _leituraService.GetLeituraPorLivroUsuario(_livroSelecionado.Id, _usuario.Id);
            if (primeiraLeitura != null && primeiraLeitura.Status == StatusLeitura.Concluida)
            {
                await _dialogoService.Mensagem("Leitura já realizada.", $"Você já realizaou a leitura do livro {_livroSelecionado.Titulo}.\nEscolha novo livro", "OK");
                return;
            }
            _leitura.LivroId = _livroSelecionado.Id;
            _leitura.UsuarioId = _usuario.Id;
            await _leituraService.SalvarLeitura(_leitura);
            await _dialogoService.Mensagem("Leitura iniciada.", $"Você iniciou a leitura de {_livroSelecionado.Titulo}.", "OK");
        }

        //ajuste de visivibilidade
        public void AjustarVisibilidade(bool iniciar)
        {
            if (iniciar)
            {
                ProximoLivro = true;
                LivroAtual = false;
            }
            else
            {
                ProximoLivro = false;
                LivroAtual = true;
            }
        }        
    }
}
