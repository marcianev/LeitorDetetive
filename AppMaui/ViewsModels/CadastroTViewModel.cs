using AppMaui.Core.Models;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Local;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace AppMaui.ViewsModels
{
    public partial class CadastroTViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? nome;
        [ObservableProperty]
        private string? tamanhoTrilha;
        [ObservableProperty]
        private bool modoAlterar;
        [ObservableProperty]
        private string? mensagem;
        [ObservableProperty]
        private Turma turma;
        
        public Usuario _usuario;

        public Action? OnFecharCadastro { get; set; }

        private readonly TurmaService _turmaService;
        private readonly ProfessorService _professorService;

        public CadastroTViewModel(TurmaService turmaService, ProfessorService professorService)
        {
            _turmaService = turmaService;
            _professorService = professorService;
            turma = new Turma();
            ModoAlterar = true;
            _usuario = SessaoService.UsuarioLogado ?? new Usuario();
        }

        //metodo salvar turma
        [RelayCommand]
        private async Task SalvarTurma()
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                Mensagem = "O nome da turma é obrigatório.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }

            if (string.IsNullOrWhiteSpace(TamanhoTrilha))
            {
                Mensagem = "O tamanho da trilha é obrigatório.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }

            Turma.Nome = Nome;
            Turma.TamanhoTrilha = int.TryParse
                (TamanhoTrilha?.Split(' ')[0], out int tamanho) ? tamanho : 0;
            
                        
            if (Turma.Id != 0)
            {
                var res = await _turmaService.AtualizarTurma(Turma);
                if (res)
                    Mensagem = "Turma atualizada com sucesso!";
                else
                {
                    Mensagem = "Erro ao atualizar turma.";                                   
                }                   
            }
            else
            {
                var professor = await _professorService.BuscarProfessorPorUsuario(_usuario.Id);
                Turma.ProfessorId = professor?.Id ?? 0;
                Turma.TrilhaId = 1;
                var resul = await _turmaService.SalvarTurma(Turma);
                if (resul)
                    Mensagem = "Turma salva com sucesso!";
                else
                {
                    Mensagem = "Erro ao salvar turma.";                                    
                }                   
            }

            await Task.Delay(3000);
            Mensagem = string.Empty;
            Nome = "";
            FecharCadastro();
        }

        //comando arquiva turma
        [RelayCommand]
        public async Task ArquivarTurma()
        {
            if (Turma.Id <= 0)
            {
                Mensagem = "Turma inválida para arquivamento.";
                await Task.Delay(3000);
                Mensagem = string.Empty;
                return;
            }
            Turma.Status = false;
            var res = await _turmaService.AtualizarTurma(Turma);
            if (res)
                Mensagem = "Turma arquivada com sucesso!";
            else
                Mensagem = "Erro ao arquivar turma.";
            await Task.Delay(3000);
            Mensagem = string.Empty;
            FecharCadastro();
        }

        [RelayCommand]
        private void FecharCadastro()
        {
            OnFecharCadastro?.Invoke();
            Turma = new();
            Nome = string.Empty;
            TamanhoTrilha = null;
        }
    }
}
