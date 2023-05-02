using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace OppSwap.ViewModels
{
    public partial class JoinPageViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<string> games;

        [ObservableProperty]
        string gameCode;

        [ObservableProperty]
        ObservableCollection<Room> gamesJoined;

        



        public JoinPageViewModel()
        {
            games = new ObservableCollection<string>();
            gamesJoined = new ObservableCollection<Room>(/*ClientInterconnect.c.gamesJoined.Keys*/);
        }


        [RelayCommand]
        void Add()
        {
            if (string.IsNullOrWhiteSpace(GameCode))
            {
                return;
            }
            Games.Add(GameCode);
            //GamesJoined.Add(GameCode);
            GameCode = string.Empty;
        }


        [RelayCommand]
        void joinRoom()
        {
            if (string.IsNullOrWhiteSpace(GameCode))
            {
                return;
            }
            ClientInterconnect.JoinGame(GameCode);
            GameCode = "";
            //call raj's thing with the game code;

        }


        [RelayCommand]
        async Task goToRoomDetailPage()
        {
            ClientInterconnect.c.JoinGame(GameCode);

            await gameJoined(GameCode);
            await Shell.Current.GoToAsync(nameof(RoomDetailPage),
            new Dictionary<string, object>
            {
                //get the room we made with the textbox inside of it
                ["CurrRoom"] = ClientInterconnect.getRoom(GameCode)
            });
        }
        private static async Task gameJoined(String s)
        {
            while (true)
            {
                await Task.Delay(100);
                foreach (Room r in ClientInterconnect.c.gamesJoined.Values)
                {
                    if (r.Id.Equals(s))
                    {
                        return;
                    }
                }
            }
        }
    }
}

