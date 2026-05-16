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
        [ObservableProperty]
        private bool botaoEdicao;
        [ObservableProperty]
        private bool modoEdicao;
        public Usuario _usuario;
        [ObservableProperty]
        private bool mostrarCadastro;        
        
        public CadastroPViewModel CadastroPVM { get; }

        DashPView  _dashPView;
        AlunoPView _alunoPView;
        TurmaPView _turmaPView;
        Professor _professor;
        ProfessorService _professorService;

        public PaginaBaseViewModel(DashPView dashPView, 
            AlunoPView alunoPView, TurmaPView turmaPView,
            CadastroPViewModel cadastroPVM, ProfessorService professorService)
        {
            _dashPView = dashPView;
            _alunoPView = alunoPView;
            _turmaPView = turmaPView;
            CadastroPVM = cadastroPVM;
            _professorService = professorService;
            _usuario = new Usuario();
            Logado();
            CadastroPVM.OnFecharCadastro = () => MostrarCadastro = false;
            if (_usuario.Tipo == "Professor")
                BotaoEdicao = true;
            ModoEdicao = false;
            AbrirHome();
            _professor = new();
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

        //mostrar view de edição de usuário
        [RelayCommand]
        private async Task AbrirEdicaoUser()
        {
            _professor = await _professorService.BuscarProfessorPorUsuario(_usuario.Id);
            if (_professor == null)
                throw new Exception("Professor não encontrado.");
            else
            {
                CadastroPVM.Id = _professor.Id;
                CadastroPVM.User = _usuario.User;
                CadastroPVM.UserTemp = _usuario.User;
                CadastroPVM.Nome = _professor.Nome;
                CadastroPVM.Email = _professor.Email;
                CadastroPVM.Cpf = _professor.Cpf;
                CadastroPVM.UsuarioId = _usuario.Id;
                CadastroPVM.ModoAlterar = true;
                MostrarCadastro = true;
            }
            
        }

        

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
