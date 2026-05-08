using AppMaui.Views;

namespace AppMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();            
            Routing.RegisterRoute("PaginaBase", typeof(PaginaBase));
            Routing.RegisterRoute("Login", typeof(Login));

            
        }
    }
}
