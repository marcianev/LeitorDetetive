using AppMaui.Core.Data;
using AppMaui.Core.Repositories;
using AppMaui.Core.Services.Application;
using AppMaui.Core.Services.External;
using AppMaui.Core.Services.Interfaces;
using AppMaui.Core.Services.Local;
using AppMaui.Core.Services.Logging;
using AppMaui.Core.Services.Security;
using AppMaui.Core.Services.Validation;
using AppMaui.Core.Settings;
using AppMaui.Services;
using AppMaui.Services.Interfaces;
using AppMaui.Views;
using AppMaui.ViewsModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace AppMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //congifurar o email settings
            //cria configuração
            var config = new ConfigurationBuilder()
                .AddUserSecrets<EmailSettings>()
                .Build();

            // pega os dados diretamente
            var emailSettings = config
                .GetSection("EmailSettings")
                .Get<EmailSettings>();

            //registra direto no DI
            builder.Services.AddSingleton(emailSettings);

            //cria os serviços de injeção de dependência para o banco de dados e os repositórios
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<AvaliacaoRepository>();
            builder.Services.AddSingleton<DesafioRepository>();
            builder.Services.AddSingleton<LeituraRepository>();
            builder.Services.AddSingleton<LivroRepository>();
            builder.Services.AddSingleton<LogRepository>();
            builder.Services.AddSingleton<MensagemRepository>();
            builder.Services.AddSingleton<NotificacaoRepository>();
            builder.Services.AddSingleton<PatenteRepository>();
            builder.Services.AddSingleton<ProfessorRespository>();
            builder.Services.AddSingleton<RespostaRepository>();
            builder.Services.AddSingleton<TrilhaRepository>();
            builder.Services.AddSingleton<TurmaRepository>();
            builder.Services.AddSingleton<UsuarioRepository>();

            //cria os serviços de injeção de dependência para os serviços de negócio
            builder.Services.AddSingleton<AlunoService>();
            builder.Services.AddSingleton<AvaliacaoService>();
            builder.Services.AddSingleton<DesafioService>();
            builder.Services.AddSingleton<LeituraService>();
            builder.Services.AddSingleton<LivroService>();
            builder.Services.AddSingleton<LogService>();
            builder.Services.AddSingleton<MensagemService>();
            builder.Services.AddSingleton<NotificacaoService>();
            builder.Services.AddSingleton<PatenteService>();
            builder.Services.AddSingleton<ProfessorService>();
            builder.Services.AddSingleton<RespostaService>();
            builder.Services.AddSingleton<TrilhaService>();
            builder.Services.AddSingleton<TurmaService>();
            builder.Services.AddSingleton<UsuarioService>();            
            builder.Services.AddSingleton<CadastrarProfessorService>();
            builder.Services.AddSingleton<EmailService>();   
            builder.Services.AddSingleton<SenhaService>();
            builder.Services.AddSingleton<OpenAIService>();
           

            //registrar viewmodels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<CadastroPViewModel>();
            builder.Services.AddTransient<NovaSenhaViewModel>();
            builder.Services.AddTransient<PAcessoViewModel>();
            builder.Services.AddTransient<PaginaBaseViewModel>();

            //registrar as interfaces
            builder.Services.AddSingleton<IAutenticacaoService, AutenticacaoService>();            
            builder.Services.AddSingleton<ICriptoService, CriptoService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IValidationService, ValidationService>();

            //registrar paginas
            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<PaginaBase>();

            //registrar as views
            builder.Services.AddTransient<CadastroPView>();
            builder.Services.AddTransient<PrimeiroAcessoView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
