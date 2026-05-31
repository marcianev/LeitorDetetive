namespace ApiBackend.Services.Email
{
    public interface IEmailService
    {
        Task<bool> EnviarEmail(string destino, string assunto, string mensagem);
    }
}
