using KemperTestCodeMaui.ViewModels;

namespace KemperTestCodeMaui;

public partial class HUD : ContentPage
{
	public HUD(HUDViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
           
    }
}