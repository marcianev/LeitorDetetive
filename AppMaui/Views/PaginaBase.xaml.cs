using AppMaui.Core.Services;
using AppMaui.ViewsModels;

namespace AppMaui.Views;

public partial class PaginaBase : ContentPage
{
	private readonly PaginaBaseViewModel _vm;
	public PaginaBase(PaginaBaseViewModel viewModel)
	{
		InitializeComponent();
		_vm = viewModel;
		BindingContext = viewModel;
	}   
}