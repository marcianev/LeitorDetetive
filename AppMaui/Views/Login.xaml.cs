using AppMaui.Core.Data;
using AppMaui.ViewsModels;

namespace AppMaui.Views;

public partial class Login : ContentPage
{   
    public Login(LoginViewModel viewModel )
    {
        InitializeComponent();        
        BindingContext = viewModel;        
    }    
}