using System.Collections.ObjectModel;

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace OppSwap.ViewModels
{
    [QueryProperty(nameof(JoinedGames)/*The name of the property in ViewModel*/, nameof(JoinedGames)/*The name of the property when you switch pages*/)]
    public partial class JoinPageViewModel : ObservableObject
    {
        //[ObservableProperty]
        //ObservableCollection<string> games;

        [ObservableProperty]
        string gameCode;

        [ObservableProperty]
        Dictionary<string,Room> joinedGames;

        [ObservableProperty]
        List<String> roomNames = new List<String>();

        public JoinPageViewModel()
        {
            //games = new ObservableCollection<string>();
            foreach(Room r in ClientInterconnect.c.gamesJoined.Values)
            {
                RoomNames.Add(r.Name + "\n" + r.Id);
            }
        }
        [RelayCommand]
        async void back()
        {
            await Shell.Current.GoToAsync(nameof(MainPage2));
        }

        [RelayCommand]
        async Task Tap(String s)
        {
            string temp = s.Split()[1];
            //ClientInterconnect.c.JoinGame(r.Id);
            Room r = ClientInterconnect.getRoom(temp);
            //await gameJoined(GameCode);
            if (r.started == true)
            {
                if (r.Winner != null)
                {
                    await Shell.Current.GoToAsync(nameof(RoomDetailPage),
                    new Dictionary<string, object>
                    {
                        //get the room we made with the textbox inside of it
                        ["Winner"] = r.Winner.Name+" won the Game" 
                    });
                }
                else if (r.IsAlive)
                {
                    await Shell.Current.GoToAsync(nameof(RoomDetailPage),
                    new Dictionary<string, object>
                    {
                        //get the room we made with the textbox inside of it
                        ["CurrRoom"] = r
                    });
                }
                else
                {
                    //TODO display dead screen
                    await Shell.Current.GoToAsync(nameof(DeadPage),
                    new Dictionary<string, object>
                    {
                        //get the room we made with the textbox inside of it
                        ["CurrRoom"] = r
                    });
                }
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(StartGamePage),
                new Dictionary<string, object>
                {
                    ["CurrRoom"] = r
                });
            }
        }


        //TODO never used i think
        [RelayCommand]
        async Task Delete(Room r)
        {
            ClientInterconnect.c.gamesJoined.Remove(r.Id);
            await Shell.Current.GoToAsync(nameof(JoinPage),
           new Dictionary<string, object>
           {
               //get the room we made with the textbox inside of it
               ["JoinedGames"] = ClientInterconnect.c.gamesJoined
           }) ;
        }

        //TODO never used i think
        [RelayCommand]
        async void joinRoom()
        {
            if (string.IsNullOrWhiteSpace(GameCode))
            {
                return;
            }
            ClientInterconnect.JoinGame(GameCode);
            await Task.Delay(600);
            await Shell.Current.GoToAsync(nameof(JoinPage),
            new Dictionary<string, object>
            {
                //get the room we made with the textbox inside of it
                ["JoinedGames"] = ClientInterconnect.c.gamesJoined
            });
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
