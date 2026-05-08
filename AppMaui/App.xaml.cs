using AppMaui.Core.Data;

namespace AppMaui
{
    public partial class App : Application
    {
        public App(DatabaseService db)
        {
            InitializeComponent();
            InicializarBanco(db);
        }
        private static async void InicializarBanco(DatabaseService db)
        {
            await db.CriarTabelas();
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