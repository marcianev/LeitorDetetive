using AppMaui.Core.Data;
using AppMaui.Core.Services.Local;

namespace AppMaui
{
    public partial class App : Application
    {
        public App(DatabaseService db, LivroService livroService, 
            DesafioService desafioService)
        {
            InitializeComponent();
            InicializarBanco(db, livroService, desafioService);
        }
        private static async void InicializarBanco(DatabaseService db, LivroService livroService,
            DesafioService desafioService)
        {
            await db.CriarTabelas();
            await livroService.PopularLivros();
            await desafioService.PopularDesafios();
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