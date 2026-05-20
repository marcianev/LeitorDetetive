using AppMaui.ViewsModels;

namespace AppMaui.Views;

public partial class DesafioView : ContentView
{
	public DesafioView(DesafioViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}