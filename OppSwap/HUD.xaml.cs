using OppSwap.ViewModels;

namespace OppSwap;

public partial class HUD : ContentPage
{
	public HUD(HUDViewModel vm)	
	{
		InitializeComponent();
		BindingContext = vm;
           
    }
}