using AppMaui.Core.DTOs;
using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
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
        private ObservableCollection<AvaliacaoDTO> comentarios = [];

        private readonly LivroService _livroService;
        private readonly AvaliacaoService _avaliacaoService;       

        public AvaliacaoAViewModel(LivroService livroService, 
            AvaliacaoService avaliacaoService)
        {
            _livroService = livroService;
            _avaliacaoService = avaliacaoService;
           
            Inicializar();
        }

        //carregar os dados iniciais
        public async Task Inicializar()
        {
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
            
;        }

        //carregar avaliações dto
        public async Task CarregarComentarios(int idLivro)
        {
            Comentarios.Clear();

            var avaliacoes = await _avaliacaoService.ListarPorLivro(idLivro);
            if (avaliacoes == null)
                return;    

            foreach(var avaliacao in avaliacoes)
            {
                Comentarios.Add(avaliacao);               
            }
        }

    }
}
