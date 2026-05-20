using AppMaui.Core.DTOs;
using AppMaui.Core.Models;
using AppMaui.Core.Repositories;
using AppMaui.Core.Services.Application;
using AppMaui.Core.Services.Interfaces;
using AppMaui.Core.Services.Local;
using AppMaui.Services.Interfaces;
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
       private readonly IDialogoService _dialogoService;
              
       

        public CadastroAViewModel(CadastroAlunoService cadAs, AlunoService alunoService,
            UsuarioService usuarioService, IDialogoService dialogoService)
        {           
            _alunoService = alunoService;
            _usuarioService = usuarioService;
            _dialogoService = dialogoService;
            Aluno = new();
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
                bool confirmacao = await _dialogoService.Confirmar(
                    "Confirmação", "Deseja atualizar o cadastro do aluno"+ Nome + "?", 
                    "Sim", "Não");
                if(confirmacao)
                {
                    var resp = await _alunoService.AtualizarAluno(Aluno);
                    if (resp)
                        Mensagem = "Cadastro atualizado";
                    else
                        Mensagem = "Erro ao atualizar aluno";
                }
                            
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
        private async Task DeletarAluno()
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
        private async Task ResetarSenha()
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

        [RelayCommand]
        private async Task ArquivarAluno()
        {
            if (Aluno.Id <= 0)
            {
                Mensagem = "Aluno inválido para arquivamento.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }
            var usuario = await _usuarioService.BuscarUsuarioPorId(Aluno.UsuarioId);
            if (usuario != null) 
                usuario.StatusUsuario = false;

            var res = await _usuarioService.AtualizarUsuario(usuario);
            if (res)
                Mensagem = "Aluno arquivado com sucesso!";
            else
                Mensagem = "Erro ao arquivar aluno.";
            await Task.Delay(3000);
            Mensagem = string.Empty;
            FecharCadastro();
        }

        //fechar view
        [RelayCommand]
        private void FecharCadastro()
        {
            Aluno = new();
            Nome = string.Empty;
            OnFecharCadastro?.Invoke();            
        }
        

        
    }
}
