using AppMaui.Core.Data;
using AppMaui.Core.Services.Local;

namespace AppMaui
{
    public partial class App : Application
    {
        public App(DatabaseService db, LivroService livroService, 
            DesafioService desafioService, PatenteService patenteService,
            MensagemService mensagemService)
        {
            InitializeComponent();
            InicializarBanco(db, livroService, desafioService, patenteService, mensagemService);
        }
        private static async void InicializarBanco(DatabaseService db, LivroService livroService,
            DesafioService desafioService, PatenteService patenteService, MensagemService mensagemService)
        {
            await db.CriarTabelas();
            await livroService.PopularLivros();
            await desafioService.PopularDesafios();
            await patenteService.PopularPatentes();
            await mensagemService.PopularMensagens();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var pagina = new Window(new AppShell());

#if WINDOWS
                pagina.Width = 1000;
                pagina.Height = 650;               
  
#endif


            return pagina;

        }
    }
}