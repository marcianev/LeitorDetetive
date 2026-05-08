using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using AppMaui.Core.Services.Application;
using AppMaui.Core.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.ViewsModels
{
    public partial class CadastroPViewModel : ObservableObject
    {
        //propriedades
        [ObservableProperty]
        private string? nome;
        [ObservableProperty]
        private string? user;
        [ObservableProperty]
        private string? email;
        [ObservableProperty]
        private string? cpf;
        [ObservableProperty]
        private string? mensagem;

        public Action? OnFecharCadastro { get; set; }

        //injeta o serviço de professor 
        private CadastroProfessorDTO _dto;
        private readonly CadastrarProfessorService _cps;
        //private readonly InteligenciaServico ins;
        private readonly IValidationService _validationService;
        private readonly UsuarioRepository _usuarioRepository;

        public CadastroPViewModel(CadastrarProfessorService cadPs, IValidationService validationService, UsuarioRepository usuarioRepository)
        {
            //ins = new InteligenciaServico();
            _dto = new CadastroProfessorDTO();
            _cps = cadPs;
            _validationService = validationService;
            _usuarioRepository = usuarioRepository;
        }

        //teste
        public async Task TestarIA()
        {
            //var resposta = await ins.TestarIA();
            //Mensagem = resposta;
            //Debug.WriteLine(resposta);
            //await Task.Delay(3000);
           // Mensagem = "";


        }

        //comoando para salvar professor
        [RelayCommand]
        private async Task SalvarProfessor()
        {            
            //valida exitência
            if (string.IsNullOrWhiteSpace(Nome) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Cpf) ||
                string.IsNullOrWhiteSpace(User))
            {
                Mensagem = "Todos os campos são obrigatórios";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }

            //chama o validar cpf
            bool valCpf = _validationService.ValidarCPF(Cpf);
            if (!valCpf)
            {
                Mensagem = "CPF inválido, digite um valor válido.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }

            //Chama o validar email
            bool valEmail = _validationService.ValidarEmail(Email);
            if (!valEmail)
            {
                Mensagem = "Email inválido, digite um valor válido.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }


            //carrega o DTO
            _dto = new CadastroProfessorDTO
            {
                Nome = Nome,
                User = User,
                Email = Email,
                Cpf = Cpf,
                Tipo = "Professor"
            };

            Mensagem = await _cps.CadastrarProfessor(_dto);
            await Task.Delay(3000);
            Mensagem = string.Empty;
            FecharCadastro();
        }


        [RelayCommand]
        private void FecharCadastro()
        {
            OnFecharCadastro?.Invoke();
        }
    }
}
