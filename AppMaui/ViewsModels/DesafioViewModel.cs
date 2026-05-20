using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using AppMaui.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.ViewsModels
{
    public partial class DesafioViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? capa;
        [ObservableProperty]
        private string? titulo;
        [ObservableProperty]
        private string? palavraSecreta;     
        [ObservableProperty]
        private ObservableCollection<DesafiosDTO>? desafiosDTOEsquerda = [];
        [ObservableProperty]
        private ObservableCollection<DesafiosDTO>? desafiosDTODireita = [];
        [ObservableProperty]
        private ObservableCollection<LetraDTO>? palavraSecretaLetras = [];
        
        private readonly DesafioService _desafioService;
        private readonly LivroService _livroService;
        private readonly RespostaService _respostaService;
        private readonly LeituraService _leituraService;
        private IDialogoService _idialogoService;
        private readonly AlunoService _alunoService;       
        private readonly Usuario? _usuario = new();
        private readonly CriptogramaService _criptogramaService;

        public DesafioViewModel(DesafioService desafioService, LivroService livroService,
            RespostaService respostaService, LeituraService leituraService,
            IDialogoService dialogoService, AlunoService alunoService,
            CriptogramaService criptogramaService)
        {
            _desafioService = desafioService;
            _livroService = livroService;
            _respostaService = respostaService;
            _leituraService = leituraService;
            _idialogoService = dialogoService;
            _alunoService = alunoService;
            _criptogramaService = criptogramaService;
            _usuario = SessaoService.UsuarioLogado;
            DefinirLivro();
        }

        public async Task DefinirLivro()
        {
            //buscar leitura atual
            if (_usuario != null)
            {
                var leitura = await _leituraService.GetLeituraAtualPorUsuario(_usuario.Id);
                if (leitura == null)
                    await _idialogoService.Mensagem("Sem Leitura Iniciada", "Inicie uma leitura para ver o desafio", "OK");
                else
                {
                    //busca dados do livro, do aluno e dos desafios e respostas para o livro
                    var livro = await _livroService.BuscarLivroPorId(leitura.LivroId);
                    var aluno = await _alunoService.BuscarAlunoUsuario(_usuario.Id);
                    var desafios = await _desafioService.ListarDesafiosPorLivro(livro.Id);
                    var respostas = await _respostaService.ListarAlunoDesafio(livro.Id, aluno.Id);
                                        
                    Capa = livro.Capa;
                    Titulo = livro.Titulo;    

                    DesafiosDTOEsquerda.Clear();
                    DesafiosDTODireita.Clear();     
                    PalavraSecretaLetras.Clear();

                    int indice = 0;

                    foreach (var d in desafios)
                    {
                        int posicao = 0;

                        var letraDTO = new ObservableCollection<LetraDTO>();

                        foreach (var letra in d.Resposta)
                        {
                            var letraDestaque = false;
                            if (posicao == d.IndicesPalavraSecreta)
                                letraDestaque = true;

                            var lDTO = new LetraDTO
                            {
                                LetraOriginal = letra.ToString(),
                                LetraCriptograma = await _criptogramaService.Criptografar(letra.ToString()),
                                Posicao = posicao,
                                Destacada = letraDestaque
                            };

                            letraDTO.Add(lDTO);
                            posicao++;
                        }

                        if (d.TipoDesafio == "PALAVRA")
                        {
                            PalavraSecreta = d.Resposta;
                            PalavraSecretaLetras.Clear();
                            foreach (var letra in d.Resposta)
                            {
                                PalavraSecretaLetras.Add(new LetraDTO
                                {
                                    LetraOriginal = letra.ToString(),   
                                });                                
                            }
                        }
                        else                       
                        {                          
                            var resposta = respostas?.FirstOrDefault(r => r.DesafioId == d.Id);
                            var respId = resposta?.Id ?? 0;
                            var respAluno = resposta?.Palavra ?? string.Empty; 

                            var dto = new DesafiosDTO
                            {
                                IdDesafio = d.Id,
                                IdLivro = livro.Id,
                                IdAluno = aluno.Id,
                                IdResposta = respId,
                                RespostaCorreta = d.Resposta,
                                Pergunta = d.Pergunta,                               
                                RespostaAluno = respAluno,
                                Letras = letraDTO
                            };    
                            if (indice < 5)
                                DesafiosDTOEsquerda.Add(dto);
                            else
                                DesafiosDTODireita.Add(dto);
                        }
                        indice++;
                    }
                }
            }
        }//definir livro       
    }
}