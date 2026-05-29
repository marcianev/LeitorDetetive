
namespace AppMaui.Core.Services.Interfaces
{
    /// <summary>
    /// Define contrato para validações de dados.
    /// </summary>
    public interface IValidationService
    {
        bool ValidarCPF(string cpf);

        bool ValidarEmail(string email);

        bool ValidarNome(string nomeCompleto);
    }
}
