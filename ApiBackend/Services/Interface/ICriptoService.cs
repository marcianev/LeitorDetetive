namespace ApiBackend.Services.Interface
{
    public interface ICriptoService
    {
        string GerarHash(string senha);
        bool VerificarHash(string senhaDigitada, string hashSalvo);
    }
}
