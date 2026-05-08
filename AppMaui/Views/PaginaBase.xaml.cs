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
	
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		bool confirmar = await DisplayAlert(
			"Teste IA",
            "Desejar validar a conexão com a IA?\nOi Marta, vc é muita burra, não fale mais comigo.",
			"Sim",
			"Não");
		if (confirmar)
			await _vm.ValidacaoAutomaticaCommand.ExecuteAsync(null);
	}
}