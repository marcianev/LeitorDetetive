using AppMaui.Core.DTOs;
using AppMaui.Core.Enums;
using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using AppMaui.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
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
        [ObservableProperty]
        private Color corBorda;

        private Usuario usuario;

        public CadastroCViewModel CadastroCVM{ get; set; }
       
        private readonly LivroService _livroService;
        private readonly AvaliacaoService _avaliacaoService;    
        private readonly LeituraService _leituraService;
        private readonly IDialogoService _dialogoService;

        public AvaliacaoAViewModel(LivroService livroService, 
            AvaliacaoService avaliacaoService, CadastroCViewModel cadastroCViewModel,
            LeituraService leituraService, IDialogoService dialogoService)
        {
            _livroService = livroService;
            _avaliacaoService = avaliacaoService;
            _leituraService = leituraService;
            _dialogoService = dialogoService;
            CadastroCVM = cadastroCViewModel;
            CadastroCVM.OnFecharCadastro = () => MostrarCadastro = false;

            Inicializar();
        }

        //carregar os dados iniciais
        public async Task Inicializar()
        {
            usuario = SessaoService.UsuarioLogado;
            if(usuario.Tipo == "Professor")            
                corBorda = (Color)Application.Current!.Resources["palhaMedio"];
            else if (usuario.Tipo == "Aluno")
                corBorda = (Color)Application.Current!.Resources["verdeMedio"];

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
           if(usuario.Tipo == "Aluno")
            {
                var concluido = await _leituraService.GetLeituraPorLivroUsuario(idLivro, usuario.Id);
                if (concluido == null)
                    Comentar = false;
                else if (concluido.Status != StatusLeitura.Concluida)
                    Comentar = false;
                else
                    Comentar = true;
            }           

            ComentariosE.Clear();
            ComentariosD.Clear();

            var avaliacoes = await _avaliacaoService.ListarPorLivro(idLivro);
            if (avaliacoes == null)
                return;    

            foreach(var avaliacao in avaliacoes)
            {
                if (avaliacao.Status == StatusAvaliacao.Aprovada || usuario.Tipo == "Professor")
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

        //abrir moderação
        [RelayCommand]
        private async Task ModerarComentario(AvaliacaoDTO comentario)
        {          
            if (usuario.Tipo != "Professor" || comentario == null || comentario.Status == StatusAvaliacao.Aprovada)
                return;

            var resposta = await _dialogoService.Consulta3(
                "Deseja aprovar este comentário?",
                "Cancelar",
                "Excluir",
                "Aprovar");

            switch (resposta)
            {
                case "Aprovar":
                    comentario.Status = StatusAvaliacao.Aprovada;
                    await _avaliacaoService.AtualizarAvaliacao(comentario);
                    break;
                case "Excluir":
                    await _avaliacaoService.DeletarAvaliacao(comentario.IdAvaliacao);
                    break;
                case "Cancelar":
                    break;
            }
        }

}
}
