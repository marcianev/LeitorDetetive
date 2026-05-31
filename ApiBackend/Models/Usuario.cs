namespace ApiBackend.Models
{
    public class Usuario
    {

        public int Id { get; set; }

        public string User { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public bool StatusSenha { get; set; }

        public bool StatusUsuario { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Tipo { get; set; } = string.Empty;
    }
}
