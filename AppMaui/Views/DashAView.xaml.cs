using AppMaui.ViewsModels;

namespace AppMaui.Views;

public partial class DashAView : ContentView
{
	public DashAView(DashAViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}