using AppMaui.ViewsModels;

namespace AppMaui.Views;

public partial class EstanteAView : ContentView
{   
    public EstanteAView(EstanteViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}   
}