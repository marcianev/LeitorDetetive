using AppMaui.ViewsModels;

namespace AppMaui.Views;

public partial class AvaliacaoAView : ContentView
{
	public AvaliacaoAView(AvaliacaoAViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

	}
}