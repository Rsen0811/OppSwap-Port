using System.Collections.ObjectModel;

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace OppSwap.ViewModels
{
    [QueryProperty(nameof(FetchedGames)/*The name of the property in ViewModel*/, nameof(FetchedGames)/*The name of the property when you switch pages*/)]
    public partial class FetchedGamesPageViewModel: ObservableObject
    {
        //[ObservableProperty]
        //ObservableCollection<string> games;

        [ObservableProperty]
        string gameCode;

        [ObservableProperty]
        List<string> roomNames=new List<string>()   ;

        [ObservableProperty]
        List<Room> fetchedGames;        

        public FetchedGamesPageViewModel()
        {
            foreach (Room r in ClientInterconnect.c.fetchedRooms)
            {
                RoomNames.Add(r.Name+"\n"+ r.Id);
            }
            //games = new ObservableCollection<string>();
        }
        

        [RelayCommand]
        async Task Tap(String s)
        {
            string temp= s.Split()[1];
            //ClientInterconnect.c.JoinGame(r.Id);
            //await gameJoined(GameCode);
            ClientInterconnect.JoinGame(temp);
            await gameJoined(temp);
            await Shell.Current.GoToAsync(nameof(JoinPage),
            new Dictionary<string, object>
            {
                //get the room we made with the textbox inside of it
                ["JoinedGames"] = ClientInterconnect.c.gamesJoined
            }); ;
        }


  
        [RelayCommand]
        async Task Delete(Room r)
        {

            ClientInterconnect.c.gamesJoined.Remove(r.Id);
            await Shell.Current.GoToAsync(nameof(JoinPage),
           new Dictionary<string, object>
           {
               //get the room we made with the textbox inside of it
               ["JoinedGames"] = ClientInterconnect.getRoom(GameCode)
           });
        }


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
               ["JoinedGames"] = ClientInterconnect.c.gamesJoined.Values
           }) ;


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
