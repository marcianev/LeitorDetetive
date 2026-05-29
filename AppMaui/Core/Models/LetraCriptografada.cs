
namespace AppMaui.Core.Models
{
    /// <summary>
    /// Representa o mapeamento entre uma letra original e seu símbolo criptografado.
    /// </summary>
    public class LetraCriptografada
    {
        public string LetraOriginal { get; set; } = string.Empty;

        public string Simbolo { get; set; } = string.Empty;
    }
}
