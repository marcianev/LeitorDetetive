using AppMaui.ViewsModels;

namespace AppMaui.Views;

public partial class AlunoPView : ContentView
{
	public AlunoPView(AlunoPViewModel view)
	{
		InitializeComponent();
		BindingContext = view;
	}   
}