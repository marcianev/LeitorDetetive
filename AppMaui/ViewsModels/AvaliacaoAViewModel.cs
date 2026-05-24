using AppMaui.Core.DTOs;
using AppMaui.Core.Enums;
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
    public partial class AvaliacaoAViewModel : ObservableObject
    {  
        [ObservableProperty]
        private ObservableCollection<Livro> livros = [];
        [ObservableProperty]
        private Livro livroSelecionado = new();
        [ObservableProperty]
        private ObservableCollection<AvaliacaoDTO> comentariosE = [];
        [ObservableProperty]
        private ObservableCollection<AvaliacaoDTO> comentariosD = [];
        [ObservableProperty]
        private bool mostrarCadastro;
        [ObservableProperty]
        private bool comentar;

        private int usuario;

        public CadastroCViewModel CadastroCVM{ get; set; }
       
        private readonly LivroService _livroService;
        private readonly AvaliacaoService _avaliacaoService;    
        private readonly LeituraService _leituraService;

        public AvaliacaoAViewModel(LivroService livroService, 
            AvaliacaoService avaliacaoService, CadastroCViewModel cadastroCViewModel,
            LeituraService leituraService)
        {
            _livroService = livroService;
            _avaliacaoService = avaliacaoService;
            _leituraService = leituraService;
            CadastroCVM = cadastroCViewModel;
            CadastroCVM.OnFecharCadastro = () => MostrarCadastro = false;

            Inicializar();
        }

        //carregar os dados iniciais
        public async Task Inicializar()
        {
            usuario = SessaoService.UsuarioLogado.Id;
            Comentar = false;
            var lista = await _livroService.ListarLivros();
            if (lista != null)
                Livros = new ObservableCollection<Livro>(lista);   
        }

        //detecta a escolha do livro
        partial void OnLivroSelecionadoChanged(Livro value)
        {
            if (value == null)
                return;
            CarregarComentarios(value.Id);
           

        }

        //carregar avaliações dto
        public async Task CarregarComentarios(int idLivro)
        {
           
            var concluido = await _leituraService.GetLeituraPorLivroUsuario(idLivro, usuario);
            if (concluido == null)
                Comentar = false;
            else if (concluido.Status != StatusLeitura.Concluida)
                Comentar = false;
            else
                Comentar = true;

            ComentariosE.Clear();
            ComentariosD.Clear();

            var avaliacoes = await _avaliacaoService.ListarPorLivro(idLivro);
            if (avaliacoes == null)
                return;    

            foreach(var avaliacao in avaliacoes)
            {
                if (avaliacao.Status == StatusAvaliacao.Aprovada)
                    if (avaliacao.IdAvaliacao % 2 == 0)
                        ComentariosE.Add(avaliacao);
                    else
                        ComentariosD.Add(avaliacao);
            }            
        }

        //mostra a view de cadastro
        [RelayCommand]
        private void AbrirCadastro()
        {            
            MostrarCadastro = true;
            CadastroCVM.Titulo = LivroSelecionado.Titulo;
            CadastroCVM.ViewLivroId = LivroSelecionado.Id;
            CadastroCVM.ViewUsuarioId = SessaoService.UsuarioLogado.Id;   
        }

}
}
