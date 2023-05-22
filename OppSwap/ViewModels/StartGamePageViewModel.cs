using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Numerics;
namespace OppSwap.ViewModels
{
    [QueryProperty(nameof(CurrRoom)/*The name of the property in ViewModel*/, nameof(CurrRoom)/*The name of the property when you switch pages*/)]

    public partial class StartGamePageViewModel : ObservableObject
    {

        [ObservableProperty]
        Room currRoom;

        [ObservableProperty]
        String[] playerNames;

        public StartGamePageViewModel(){}
        //only works with IOS as of right now
        [RelayCommand]
        public async Task Updates()
        {
            await Task.Delay(1000);
            while (true)
            {
                if (CurrRoom != null)
                {
                    CurrRoom = ClientInterconnect.c.gamesJoined[CurrRoom.Id];
                    PlayerNames = CurrRoom.tempholderwhileplayersdonthavenamesonserver;
                }
                await Task.Delay(10000);//wait 10 seconds
            }
        }

        [RelayCommand]
        public async void Start()
        {
            ClientInterconnect.StartGame(CurrRoom.Id);
            await Task.Delay(1000);
            await Shell.Current.GoToAsync(nameof(RoomDetailPage),
                new Dictionary<string, object>
                {
                    //get the room we made with the textbox inside of it
                    ["CurrRoom"] = ClientInterconnect.getRoom(CurrRoom.Id)
                }); ;
        }
    }
    
}

