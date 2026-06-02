using AppMaui.Core.DTOs;
using AppMaui.Core.Services.Api.Interface;
using AppMaui.Core.Services.Interfaces;
using AppMaui.Services;
using AppMaui.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared.DTOs.Requests;
using System.Diagnostics;

namespace AppMaui.ViewsModels
{
    public partial class CadastroPViewModel : ObservableObject
    {
        //propriedades
        [ObservableProperty]
        private int id;
        [ObservableProperty]
        private string? nome;
        [ObservableProperty]
        private string? user;
        [ObservableProperty]
        private string? userTemp;
        [ObservableProperty]
        private string? email;
        [ObservableProperty]
        private string? cpf;
        [ObservableProperty]
        private int usuarioId;
        [ObservableProperty]
        private string? mensagem;
        [ObservableProperty]
        private bool modoAlterar;

        public Action? OnFecharCadastro { get; set; }
        public Action<bool>? OnCarregando;

        //injeta o serviço de professor 
        private CadastroProfessorDTO _dto;
        private readonly ICadastroProfessorApiService _cadastroProfessorApiService;      
        private readonly IValidationService _validationService;
        private readonly ConectividadeService _conectividadeService;
        private readonly IDialogoService _dialogoService;

        public CadastroPViewModel(ICadastroProfessorApiService cadastroProfessorApiService, IValidationService validationService, 
           ConectividadeService conectividadeService, IDialogoService dialogoService)
        {
            
            _dto = new CadastroProfessorDTO();
            _cadastroProfessorApiService = cadastroProfessorApiService;
            _validationService = validationService;
            _conectividadeService = conectividadeService;
            _dialogoService = dialogoService;
            ModoAlterar = true;
        }        

        //comando para salvar professor
        [RelayCommand]
        private async Task SalvarProfessor()
        {
            try
            {
                OnCarregando?.Invoke(true);

                var temInternet = _conectividadeService.TemInternet();
                if(!temInternet)
                {
                    Mensagem = "É necessário conexão para novo cadastro";
                    await Task.Delay(3000);
                    Mensagem = string.Empty;
                    FecharCadastro();
                    return;
                }
                //valida existência
                if (string.IsNullOrWhiteSpace(Nome) ||
                    string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Cpf) ||
                    string.IsNullOrWhiteSpace(User))
                {
                    Mensagem = "Todos os campos são obrigatórios";
                    await Task.Delay(3000);
                    Mensagem = string.Empty;
                    FecharCadastro();
                    return;
                }
                //carrega o DTO
                var request = new CadastroProfessorRequest
                {
                    Nome = Nome,
                    User = User,
                    Email = Email,
                    Cpf = Cpf
                };
                if (Id != 0)
                {
                    bool confirmacao = await _dialogoService.Confirmar(
                    "Confirmação", $"Deseja atualizar os dados de {Nome}?",
                    "Sim", "Não");

                    if (confirmacao)
                    {
                        if (UserTemp != User)
                        {
                            _dto.User = User;
                        }
                        _dto.Id = Id;
                        _dto.UsuarioId = UsuarioId;
                    }
                    else
                        return;
                }
                else
                {
                    //chama o validar cpf
                    bool valCpf = _validationService.ValidarCPF(Cpf);
                    if (!valCpf)
                    {
                        Mensagem = "CPF inválido, digite um valor válido.";
                        await Task.Delay(3000);
                        Mensagem = string.Empty;
                        FecharCadastro();
                        return;
                    }

                    //Chama o validar email
                    bool valEmail = _validationService.ValidarEmail(Email);                    
                    if (!valEmail)
                    {
                        Mensagem = "Email inválido, digite um valor válido.";
                        await Task.Delay(3000);
                        Mensagem = string.Empty;
                        FecharCadastro();
                        return;
                    }

                    //chama o validar nome
                    bool valNome = _validationService.ValidarNome(Nome);
                    if (!valNome)
                    {
                        Mensagem = "Sobrenome, apenas letras.";
                        await Task.Delay(3000);
                        Mensagem = string.Empty;
                        FecharCadastro();
                        return;
                    }
                }
                var resultado = await _cadastroProfessorApiService.CadastrarProfessor(request);
                if(resultado == null)
                {
                    Debug.WriteLine(resultado.Mensagem);
                    Mensagem = "Erro ao cadastrar professor.";
                    await Task.Delay(3000);
                    Mensagem = string.Empty;
                    FecharCadastro();
                    return;
                }
                    
                Mensagem = resultado.Mensagem;
                await Task.Delay(3000);
                Mensagem = string.Empty;               
            }
            finally
            {
                OnCarregando?.Invoke(false);
            }
            FecharCadastro();
        }
        
        //comando para arquivar usuario
       // [RelayCommand]


        [RelayCommand]
        private void FecharCadastro()
        {
            Nome = string.Empty;
            User = string.Empty;
            Email = string.Empty;
            Cpf = string.Empty;
            OnFecharCadastro?.Invoke();
        }
    }
}
