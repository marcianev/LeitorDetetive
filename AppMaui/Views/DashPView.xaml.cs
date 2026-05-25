using AppMaui.ViewsModels;

namespace AppMaui.Views;

public partial class DashPView : ContentView
{
	public DashPView(DashPViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}