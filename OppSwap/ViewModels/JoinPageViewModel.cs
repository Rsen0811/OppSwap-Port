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
            ClientInterconnect.JoinGame(GameCode);
            GameCode = "";
			//call raj's thing with the game code;
			
		}

		[RelayCommand]
        async Task goToRoomDetailPage()
        {
			//ClientInterconnect.c.JoinGame(JoinCodeText.Text);

			//await gameJoined(JoinCodeText.Text);
			await Shell.Current.GoToAsync(nameof(RoomDetailPage),
			new Dictionary<string, object>
			{
				//get the room we made with the textbox inside of it
				["CurrRoom"] = new Room("bruh","bruh")// ClientInterconnect.getRoom(GameCode)
			});
        }
    }
}

