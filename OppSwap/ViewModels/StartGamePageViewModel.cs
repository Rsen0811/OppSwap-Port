﻿using System;
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
        List<string> playerNames;

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
                    if(!CurrRoom.Equals(ClientInterconnect.c.gamesJoined[CurrRoom.Id]))
                    CurrRoom = ClientInterconnect.c.gamesJoined[CurrRoom.Id];
                    if (CurrRoom.players != null)
                    {
                        if (PlayerNames==null || !CompareArrays(PlayerNames, PlayerToName(ClientInterconnect.c.gamesJoined[CurrRoom.Id].players)))
                        {
                            PlayerNames = PlayerToName(CurrRoom.players);
                        }
                    }
                }
                await Task.Delay(100);//wait 10 seconds
            }
        }
        private bool CompareArrays(List<string> arr1, List<string> arr2)
        {
            if (arr1.Count != arr2.Count)
            {
                return false;
            }
            for(int i = 0; i < arr1.Count; i++)
            {                
                if (string.Compare(arr1[i],arr2[i])!=0)
                {
                    return false;
                }
            }
            return true;
        }
        private List<string> PlayerToName(List<Player> players)
        {
            List<string> temp = new List<string>();
            foreach (Player p in players) 
            {
                if(!ClientInterconnect.c.clientId.Equals(p.Id)) {
                    temp.Add(p.Name);
                }
            }
            return temp;
        } 

        [RelayCommand]
        public void Start()
        {
            ClientInterconnect.StartGame(CurrRoom.Id);
        }

        public async void toRoomDetails()
        {
            await Shell.Current.GoToAsync(nameof(RoomDetailPage),
                new Dictionary<string, object>
                {
                    //get the room we made with the textbox inside of it
                    ["CurrRoom"] = ClientInterconnect.getRoom(CurrRoom.Id)
                }); ;
        }
    }
    
}

