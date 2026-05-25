
namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO utilizado para exibição de dados para cadastro de aluno
    /// </summary>
    public class CadastroAlunoDTO
    {
        //dados vinculados ao usuario
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int TurmaId { get; set; }
        public int PatenteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string CodAcess { get; set; } = string.Empty;

        //dados vinduclados ao aluno
        public string User { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public bool? StatusSenha { get; set; }
        public bool? StatusUser { get; set; }
    }
}
