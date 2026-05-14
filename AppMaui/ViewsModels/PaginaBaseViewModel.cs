using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Local;
using AppMaui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.ViewsModels
{
    public partial class PaginaBaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? user;
        [ObservableProperty]
        private View? viewAtiva;

        public Usuario _usuario;

        DashPView  _dashPView;
        AlunoPView _alunoPView;
        TurmaPView _turmaPView;

        public PaginaBaseViewModel(DashPView dashPView, AlunoPView alunoPView, TurmaPView turmaPView)
        {
            _dashPView = dashPView;
            _alunoPView = alunoPView;
            _turmaPView = turmaPView;
            Usuario usuario = new();   
            _usuario = new Usuario();
            Logado();
            AbrirHome();
        }

        //metodo carrega label com nome do usuario e armazena o id de usuario
        public void Logado()
        {
            if (SessaoService.UsuarioLogado == null)
            {
                Shell.Current.GoToAsync("Login");
                return;
            }
            _usuario = SessaoService.UsuarioLogado;
            User = _usuario.User;
        }

        //abre a view Principal no body
        [RelayCommand]
        private void AbrirHome()
        {
            if(_usuario.Tipo == "Professor")
                ViewAtiva = _dashPView;
        }

        //abre a view do primeiro botão
        [RelayCommand]
        private void AbrirPrimView()
        {          
            if(_usuario.Tipo == "Professor")
                ViewAtiva = _alunoPView;
        }

        //abre a view do segundo botão
        [RelayCommand]
        private void AbrirSegView()
        {
            if(_usuario.Tipo == "Professor")
                ViewAtiva = _turmaPView;
        }

        //abre a view do terceiro botão
        [RelayCommand]
        private void AbrirTercView()
        {
            if(_usuario.Tipo == "Professor")
                ViewAtiva = _alunoPView;
        }

        //
            /*
        [ObservableProperty]
        private string icones;
        private readonly OpenAIService _openAIService;
        private readonly CriptogramaService _criptogramaService;
        public ObservableCollection<LetraCriptografada> Letras { get; set; }

        public PaginaBaseViewModel(OpenAIService openAIService, CriptogramaService criptogramaService)
        {
            _openAIService = openAIService;
            _criptogramaService = criptogramaService;           
        }

        public void TestarCriptografia()
        {
            var lista = _criptogramaService.Gerar(40);    
            Letras = new ObservableCollection<LetraCriptografada>(lista);
            string mensagem = _criptogramaService.Criptografar("TESTE", lista);            
            string mensagem2 = _criptogramaService.Criptografar("ESPERANÇA", lista);            
            string mensagem3 = _criptogramaService.Criptografar("METODO", lista);            
            string mensagem4 = _criptogramaService.Criptografar("ALIANÇA", lista);            
            string mensagem5 = _criptogramaService.Criptografar("CASA", lista);            
            


            Icones = mensagem + "\n" + mensagem2 +"\n"+mensagem3 + "\n" + mensagem4 + "\n" + mensagem5;
        }

        [RelayCommand]
        private async Task ValidacaoAutomatica()
        {
           
                string comentario = "Oi Marta, vc é muita burra, não fale mais comigo.";
                string resposta = await _openAIService.ValidarComentario(comentario);

                if (string.IsNullOrWhiteSpace(resposta))
                    resposta = "Sem resposta";

                await Shell.Current.DisplayAlert("Validação do comentário: ", resposta, "OK");
                TestarCriptografia();

                    
        }*/


    }
}
