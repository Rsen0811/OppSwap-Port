using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace OppSwap.ViewModels
{
	public partial class JoinPageViewModel : ObservableObject
	{
		public JoinPageViewModel() {

		}

		[ObservableProperty]
		string gameCode;

		[RelayCommand]
		void joinRoom()
		{
			if (string.IsNullOrWhiteSpace(GameCode)) {
				return;
			}
            		ClientInterconnect.CreateGame(GameCode);
            		GameCode = "";
			//call raj's thing with the game code;
			
		}
	}
}

