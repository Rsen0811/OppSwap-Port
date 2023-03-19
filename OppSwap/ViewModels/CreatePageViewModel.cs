using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace OppSwap.ViewModels
{
	public partial class CreatePageViewModel : ObservableObject
	{
		public CreatePageViewModel() {

		}

		[ObservableProperty]
		string gameCode;

		[RelayCommand]
		void createRoom()
		{
			if (string.IsNullOrWhiteSpace(GameCode)) {
				return;
			}
			GameCode = "";
			//call raj's thing with the game code;

		}
	}
}

