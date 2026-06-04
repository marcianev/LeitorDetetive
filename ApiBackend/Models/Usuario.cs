using System.ComponentModel.DataAnnotations;

namespace ApiBackend.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string User { get; set; } = string.Empty;

        [Required]   
        public string Senha { get; set; } = string.Empty;

        [Required]
        public bool StatusSenha { get; set; }

        [Required]
        public bool StatusUsuario { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

        [Required]
        public string Tipo { get; set; } = string.Empty;
    }
}
