using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace OppSwap.ViewModels
{
	public partial class Page2ViewModel : ObservableObject
	{
		public Page2ViewModel() {

		}

		[ObservableProperty]
		string gameCode;

		[RelayCommand]
		void JoinRoom()
		{
			if (string.IsNullOrWhiteSpace(GameCode)) {
				return;
			}
			//call raj's thing with the game code;
		}
	}
}

