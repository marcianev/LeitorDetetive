using AppMaui.Core.Data;
using AppMaui.Core.Repositories.AppMaui.Core.Repositories;
using AppMaui.Core.Services;
using AppMaui.Core.Services.Api;
using AppMaui.Core.Services.Api.Interface;
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
                    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FontAwesome");
                });

            //*congifurar o email settings
            using var stream = FileSystem.
                OpenAppPackageFileAsync("appsettings.json").GetAwaiter().GetResult();
            //cria configuração
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .AddUserSecrets<EmailSettings>()
                .Build();

            // gerar as configs
            var emailSettings = config
                .GetSection("EmailSettings")
                .Get<EmailSettings>();
            var openAISettings = config
                .GetSection("OpenAI")
                .Get<OpenAISettings>();

            //registrar o endereço do serviço
            builder.Services.AddHttpClient<IAuthApiService, AuthApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7170/");
            });

            //registra direto no DI
            builder.Services.AddSingleton(emailSettings!);
            builder.Services.AddSingleton(openAISettings!);

            //cria os serviços de injeção de dependência para o banco de dados e os repositórios
            builder.Services.AddSingleton<AlunoRepository>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<AvaliacaoRepository>();
            builder.Services.AddSingleton<DatabaseService>();            
            builder.Services.AddSingleton<DesafioRepository>();
            builder.Services.AddSingleton<EventoRepository>();
            builder.Services.AddSingleton<LeituraRepository>();
            builder.Services.AddSingleton<LivroRepository>();            
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
            builder.Services.AddTransient<AvaliacaoService>();
            builder.Services.AddSingleton<CadastroAlunoService>();
            builder.Services.AddSingleton<CadastrarProfessorService>();
            builder.Services.AddSingleton<CriptogramaService>();
            builder.Services.AddSingleton<DesafioService>();
            builder.Services.AddSingleton<EmailService>();
            builder.Services.AddSingleton<LeituraService>();
            builder.Services.AddSingleton<LivroService>();
            builder.Services.AddSingleton<EventoService>();
            builder.Services.AddSingleton<MensagemService>();
            builder.Services.AddSingleton<NotificacaoService>();
            builder.Services.AddSingleton<OpenAIService>();
            builder.Services.AddSingleton<PatenteService>();
            builder.Services.AddSingleton<ProfessorService>();
            builder.Services.AddSingleton<RespostaService>();
            builder.Services.AddSingleton<SenhaService>();
            builder.Services.AddSingleton<SessaoService>();
            builder.Services.AddSingleton<ConectividadeService>();
            builder.Services.AddSingleton<TrilhaService>();
            builder.Services.AddSingleton<TurmaService>();
            builder.Services.AddSingleton<UsuarioService>();

            //registrar viewmodels
            builder.Services.AddTransient<AlunoPViewModel>();
            builder.Services.AddTransient<AvaliacaoAViewModel>();
            builder.Services.AddTransient<CadastroAViewModel>();
            builder.Services.AddTransient<CadastroCViewModel>();
            builder.Services.AddTransient<CadastroPViewModel>();
            builder.Services.AddTransient<CadastroTViewModel>();
            builder.Services.AddTransient<DashAViewModel>();
            builder.Services.AddTransient<DashPViewModel>();
            builder.Services.AddTransient<DesafioViewModel>();
            builder.Services.AddTransient<EstanteViewModel>();
            builder.Services.AddTransient<LoginViewModel>();            
            builder.Services.AddTransient<NovaSenhaViewModel>();
            builder.Services.AddTransient<PAcessoViewModel>();
            builder.Services.AddTransient<PaginaBaseViewModel>();                
            builder.Services.AddTransient<TurmaPViewModel>();
            

            //registrar as interfaces
            builder.Services.AddSingleton<IAutenticacaoService, AutenticacaoService>();            
            builder.Services.AddSingleton<ICriptoService, CriptoService>();
            builder.Services.AddSingleton<IDialogoService, DialogoService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IValidationService, ValidationService>();      
            

            //registrar paginas
            builder.Services.AddTransient<Login>();
            builder.Services.AddTransient<PaginaBase>();

            //registrar as views
            builder.Services.AddTransient<AlunoPView>();
            builder.Services.AddTransient<AvaliacaoAView>();
            builder.Services.AddTransient<CadastroAView>();
            builder.Services.AddTransient<CadastroCView>();
            builder.Services.AddTransient<CadastroPView>();
            builder.Services.AddTransient<CadastroTView>();
            builder.Services.AddTransient<DashAView>();
            builder.Services.AddTransient<DashPView>();
            builder.Services.AddTransient<DesafioView>();
            builder.Services.AddTransient<EstanteAView>();
            builder.Services.AddTransient<NovaSenhaView>();
            builder.Services.AddTransient<PrimeiroAcessoView>();   
            builder.Services.AddTransient<TurmaPView>();
            
            

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
