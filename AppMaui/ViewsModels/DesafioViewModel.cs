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
    public partial class DesafioViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? capa;
        [ObservableProperty]
        private string? titulo;
        [ObservableProperty]
        private string? palavraSecreta;
        [ObservableProperty]
        private bool botaoVisivel;
        [ObservableProperty]
        private ObservableCollection<DesafiosDTO> desafiosDTOEsquerda = [];
        [ObservableProperty]
        private ObservableCollection<DesafiosDTO> desafiosDTODireita = [];
        [ObservableProperty]
        private ObservableCollection<LetraDTO> palavraSecretaLetras = [];
        
        private readonly DesafioService _desafioService;
        private readonly LivroService _livroService;
        private readonly RespostaService _respostaService;
        private readonly LeituraService _leituraService;
        private IDialogoService _idialogoService;
        private readonly AlunoService _alunoService;       
        private readonly Usuario? _usuario = new();

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
            _usuario = SessaoService.UsuarioLogado;
            _ = DefinirLivro();
        }

        public async Task DefinirLivro()
        {            
            //buscar leitura atual
            if (_usuario != null && _usuario.Tipo == "Aluno")
            {              
                var leitura = await _leituraService.GetLeituraAtualPorUsuario(_usuario.Id);               
                if (leitura == null || leitura.Status != StatusLeitura.Iniciada)
                {
                    BotaoVisivel = false;
                    Titulo = "Inicie uma leitura para ver os desafios.";
                }  
                else
                {
                    //busca dados do livro, do aluno e dos desafios e respostas para o livro
                    var livro = await _livroService.BuscarLivroPorId(leitura.LivroId);
                    if (livro == null)
                        return;
                    var aluno = await _alunoService.BuscarAlunoUsuario(_usuario.Id);
                    if (aluno == null)
                        return;
                    var desafios = await _desafioService.ListarDesafiosPorLivro(livro.Id);
                    var respostas = await _respostaService.ListarAlunoDesafio(livro.Id, aluno.Id);
                                        
                    Capa = livro.Capa;
                    Titulo = livro.Titulo;
                    BotaoVisivel = true;

                    DesafiosDTOEsquerda.Clear();
                    DesafiosDTODireita.Clear();     
                    PalavraSecretaLetras.Clear();

                    int indice = 0;

                    foreach (var d in desafios)
                    {
                        int posicao = 0;
                        var resposta = respostas?.FirstOrDefault(r => r.DesafioId == d.Id);
                        var palavraSalva = resposta?.Palavra ?? string.Empty;                       

                        var letraDTO = new ObservableCollection<LetraDTO>();

                        foreach (var letra in d.Resposta)
                        {
                            var letraDestaque = false;
                            if (posicao == d.IndicesPalavraSecreta)
                                letraDestaque = true;

                            var lDTO = new LetraDTO
                            {
                                LetraOriginal = letra.ToString(),
                                LetraCriptograma = await CriptogramaService.Criptografar(letra.ToString(), livro.Id),
                                Posicao = posicao,
                                LetraDigitada = palavraSalva.Length > posicao ? palavraSalva[posicao].ToString() : string.Empty,
                                Destacada = letraDestaque,
                                IdDesafio = d.Id
                            };                          
                            letraDTO.Add(lDTO);
                            posicao++;
                        }
                        
                        if (d.TipoDesafio == "PALAVRA")
                        {
                            var posSecreta = 0;
                            
                            PalavraSecreta = d.Resposta;
                            PalavraSecretaLetras.Clear();
                            foreach (var letra in d.Resposta)
                            {
                                PalavraSecretaLetras.Add(new LetraDTO
                                {
                                    LetraOriginal = letra.ToString(),  
                                    LetraDigitada = palavraSalva.Length > posSecreta ? palavraSalva[posSecreta].ToString() : string.Empty,
                                    IdDesafio = d.Id
                                });
                                posSecreta++;
                            }
                           
                        }
                        else                       
                        {   
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
        
        //comando salva as respostas que estão corretas
        [RelayCommand]
        private async Task ValidarRespostas()
        {
            bool livroConcluido = true;
            string resposta = string.Empty;
            int aluno = 0;
            int desafio = 0;
            var desafioCorreto = true;

            //validar os desafios da esquerda
            foreach (var livro in DesafiosDTOEsquerda)
            {
                resposta = string.Empty;
                aluno = livro.IdAluno;                
                desafioCorreto = true;

                foreach (var letra in livro.Letras)
                {
                    if (letra.LetraOriginal == letra.LetraDigitada)
                    {
                        resposta = $"{resposta}{letra.LetraDigitada}";
                    }
                    else
                    {
                        livroConcluido = false;
                        desafioCorreto = false;                        
                    }                    
                }
                if (desafioCorreto)
                {
                    await _respostaService.SalvarResposta(new Resposta
                    {                       
                        AlunoId = livro.IdAluno,
                        DesafioId = livro.IdDesafio,
                        Palavra = resposta
                    });
                }               
            }

            //validar os desafios da direita
            foreach (var livro in DesafiosDTODireita)
            {
                desafioCorreto = true;
                resposta = string.Empty;                

                foreach (var letra in livro.Letras)
                {
                    if (letra.LetraOriginal == letra.LetraDigitada)
                    {
                        resposta = $"{resposta}{letra.LetraDigitada}";
                    }
                    else
                    {
                        livroConcluido = false;
                        desafioCorreto = false;
                    }                    
                }
                if (desafioCorreto)
                {
                    await _respostaService.SalvarResposta(new Resposta
                    {                        
                        AlunoId = livro.IdAluno,
                        DesafioId = livro.IdDesafio,
                        Palavra = resposta
                    });
                }                
            }
            resposta = string.Empty;
            desafioCorreto = true;

            //validar palavra secreta
            foreach (var letra in PalavraSecretaLetras)
            {
                desafio = letra.IdDesafio;                
                if (letra.LetraOriginal == letra.LetraDigitada)
                {
                    resposta = $"{resposta}{letra.LetraDigitada}";
                   
                }
                else
                {
                    desafioCorreto = false;
                    livroConcluido = false;
                }                           
            }
            if (desafioCorreto)
            {
                await _respostaService.SalvarResposta(new Resposta
                {
                    AlunoId = aluno,
                    DesafioId = desafio,
                    Palavra = resposta
                });
            }

            if (livroConcluido)
            {
                if (_usuario == null)
                    return;

                var concluir = await _leituraService.ConcluirLeitura(_usuario.Id);
                if (concluir)
                {
                    DesafiosDTODireita.Clear();
                    DesafiosDTOEsquerda.Clear();
                    PalavraSecretaLetras.Clear();
                    Titulo = "Inicie nova Leitura.";
                    Capa = string.Empty;
                    BotaoVisivel = false;
                }
                else
                    await _idialogoService.Mensagem("Erro", "Erro ao concluir leitura, tente mais tarde.", "OK");

            }
            else
                await _idialogoService.Mensagem("ATENÇÃO", "Apenas respostas que estão completas foram salvas.", "OK");
            
        }//validar respostas
    }
}