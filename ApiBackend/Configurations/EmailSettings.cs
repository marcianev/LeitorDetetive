namespace ApiBackend.Configurations
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;

        public int Port { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;
    }
}
