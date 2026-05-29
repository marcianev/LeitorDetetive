using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Interfaces;
using AppMaui.Core.Services.Local;
using AppMaui.Core.Services.Security;
using AppMaui.Services;
using AppMaui.Services.Interfaces;
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
    public partial class NovaSenhaViewModel : ObservableObject
    {
        [ObservableProperty]
        private string user = string.Empty;
        [ObservableProperty]
        private string email = string.Empty;
        [ObservableProperty]
        private string mensagem = string.Empty;

        public Action? OnFecharNovaSenha { get; set; }        
        private readonly UsuarioService _usuarioService;
        private readonly ProfessorService _professorService;    
        private readonly ICriptoService _criptoService;
        private readonly EmailService _emailService;
        private readonly ConectividadeService _conectividadeService;

        public NovaSenhaViewModel(UsuarioService usuarioService, ProfessorService professorService,
            ICriptoService criptoService, EmailService emailService, ConectividadeService conectividadeService)
        {
            _usuarioService = usuarioService;
            _professorService = professorService;   
            _criptoService = criptoService;
            _emailService = emailService;
            _conectividadeService = conectividadeService;
        }

        [RelayCommand]
        private void FecharNovaSenha()
        {
            OnFecharNovaSenha?.Invoke();
        }

        [RelayCommand]
        private async Task EnviarNovaSenha()
        {
            var temInternet = _conectividadeService.TemInternet();
            if (!temInternet)
            {
                Mensagem = "É necessário conexão para nova senha";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                FecharNovaSenha();
                return;
            }
            if (string.IsNullOrWhiteSpace(User) || string.IsNullOrWhiteSpace(Email))
            {
                Mensagem = "Usuário e Email são obrigatórios.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }          
           
            var usuario = await _usuarioService.BuscarUsuarioPorUser(User);
            if (usuario == null)
            {
                Mensagem = "Usuário inexistente.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }

            var professor = await _professorService.BuscarProfessorPorUsuario(usuario.Id);
            if (professor == null)
            {
                Mensagem = "Usuário não é professor.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }     

            string senhaProvisoria = SenhaService.GerarSenhaProvisoria(8);
            usuario.Senha = _criptoService.GerarHash(senhaProvisoria);
            usuario.StatusSenha = false;

            await _usuarioService.AtualizarUsuario(usuario);
            await _emailService.EnviarEmail(
                professor.Email,
                "Leitor Detive - Código de Acesso",
                $"Olá, {usuario.User}. \nSeu novo código de acesso é:{usuario.Senha}. \n" +
                $"Clique em primeiro acesso no aplicativo Leitor Detetivee altere sua senha.\n" +
                $" Atenciosamente Equipe Leitor Detetive ");

            Mensagem = "Código enviado ao seu email.";
            await Task.Delay(3000);
            Mensagem = string.Empty;
            FecharNovaSenha();      
        }
    }
  
}
