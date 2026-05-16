using AppMaui.ViewsModels;

namespace AppMaui.Views;

public partial class TurmaPView : ContentView
{
	public TurmaPView(TurmaPViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}