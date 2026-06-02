
namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// DTO utilizado para exibição de dados para cadastro de aluno
    /// </summary>
    public class CadastroProfessorDTO
    {
        //dados vinculado a entidade professor
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;

        //dados vinculados a entidade usuario
        public string User { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string SenhaProvisoria { get; set; } = string.Empty;       
        public string Tipo { get; set; } = string.Empty;
        public bool? StatusSenha { get; set; }
        public bool? StatusUser { get; set; }
    }
}
