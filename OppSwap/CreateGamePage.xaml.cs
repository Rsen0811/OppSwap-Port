using OppSwap.ViewModels;

namespace OppSwap;

public partial class CreateGamePage : ContentPage
{
	public CreateGamePage(CreateGameViewModel vm)
    {

        InitializeComponent();
        BindingContext = vm;

    }
}