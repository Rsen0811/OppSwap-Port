using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace OppSwap.ViewModels
{
    [QueryProperty(nameof(Winner)/*The name of the property in ViewModel*/, nameof(Winner)/*The name of the property when you switch pages*/)]

    public partial class WonGamePageViewModel : ObservableObject
	{
        [ObservableProperty]
        string winner;

        public WonGamePageViewModel() {

		}

	

	}
}

