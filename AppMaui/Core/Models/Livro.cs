using SQLite;

namespace AppMaui.Core.Models
{
    public class Livro
    {
        //atributos do modelo de livro, com as anotações do SQLite para definir as propriedades da
        //tabela no banco de dados
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;
        [NotNull, MaxLength(100)]
        public string Autor { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Ilustrador { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Editora { get; set; } = string.Empty;
        [MaxLength(20)]
        public string Capa { get; set; } = string.Empty;
    }
}
