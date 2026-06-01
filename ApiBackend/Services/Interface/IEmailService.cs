namespace ApiBackend.Services.Interface
{
    public interface IEmailService
    {
        Task<bool> EnviarEmail(string destino, string assunto, string mensagem);
    }
}
