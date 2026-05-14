using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using AppMaui.Core.Services.Application;
using AppMaui.Core.Services.Interfaces;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace AppMaui.ViewsModels
{
    public partial class CadastroAViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? nome;
        [ObservableProperty]
        private string? mensagem;
        [ObservableProperty]
        private bool modoAlterar;        
        [ObservableProperty]
        private Aluno aluno;
        
        public Action? OnFecharCadastro { get; set;  }

         
        private CadastroAlunoDTO _dto;
        private readonly CadastroAlunoService _cas;
        private readonly AlunoService _alunoService;
        private readonly UsuarioService _usuarioService;
       
              
       

        public CadastroAViewModel(CadastroAlunoService cadAs, AlunoService alunoService,
            UsuarioService usuarioService)
        {           
            _alunoService = alunoService;
            _usuarioService = usuarioService;
            _dto = new CadastroAlunoDTO();
            _cas = cadAs;    
            ModoAlterar = true;
        }

        //comoando para salvar aluno(+usuário) ou alterar aluno
        [RelayCommand]
        private async Task SalvarAluno()
        {                      
            //valida exitência
            if (string.IsNullOrWhiteSpace(Nome))
            {
                Mensagem = "Nome é obrigatórios";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }      

            //carrega o DTO
            _dto = new CadastroAlunoDTO
            {
                Nome = Nome,   
                Tipo = "Aluno"
            };

            //alterações para atualizar
            if (Aluno.Id != 0)
            { 
                Aluno.Nome = Nome;               
                var resp = await _alunoService.AtualizarAluno(Aluno);
                if (resp)
                    Mensagem = "Cadastro atualizado";
                else
                    Mensagem = "Erro ao atualizar aluno";                
            }
            else
            {
                Mensagem = await _cas.CadastrarAluno(_dto);
            }
            
            await Task.Delay(3000);
            Mensagem = string.Empty;
            Nome = "";
            FecharCadastro();
        }

        //comando deletar aluno
        [RelayCommand]
        private async void DeletarAluno()
        {
            if (Aluno.Id != 0)
            {
                var resp = await _alunoService.DeletarAluno(Aluno.Id);
                if (resp)
                    Mensagem = "Cadastro deletado.";
                else
                    Mensagem = "Erro ao deletar aluno.";
            }
            else
                Mensagem = "Id de aluno é obrigatória";
            await Task.Delay(3000);
            Mensagem = string.Empty;
            Nome = "";
        }

        //comando para resetar a senha do aluno 
        [RelayCommand]
        private async void ResetarSenha()
        {          
            if (Aluno.UsuarioId != 0)
            {
                var usuario = await _usuarioService.BuscarUsuarioPorId(Aluno.UsuarioId);
                if (usuario != null)
                {
                    usuario.Senha = Aluno.CodigoAcesso;                    
                    usuario.StatusSenha = false;

                    var resp = await _usuarioService.AtualizarUsuario(usuario);
                    if (resp)
                        Mensagem = "Sucesso.Clicar em primeiro acesso e usar o codigo de acesso.";
                    else
                        Mensagem = "Erro ao resetar senha do aluno.";
                }
                else
                    Mensagem = "Id de aluno é obrigatório";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                FecharCadastro();
            }
        }

        //fechar view
        [RelayCommand]
        private void FecharCadastro()
        {            
            OnFecharCadastro?.Invoke();
        }
        

        
    }
}
