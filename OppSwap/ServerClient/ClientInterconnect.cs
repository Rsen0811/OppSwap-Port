using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using OppSwap.ViewModels;

namespace OppSwap
{
    public static class ClientInterconnect
    {
#if DISABLE_SERVER // this to false if running, true if not
        public static readonly bool RUNNING_SERVER = false; 
#else
        public static readonly bool RUNNING_SERVER = true; 
#endif
        public static readonly Client c = (RUNNING_SERVER ? new Client() : null);

        private static List<String> commandList = new List<String>();
        public static LatLong position;

        public static void Ping() { commandList.Add("p"); }
        public static void CreateGame(String name) { commandList.Add("c " + name); }
        public static void JoinGame(String gameId) { commandList.Add("j " + gameId); }
        public static void FetchGames(String query) { commandList.Add("f " + query); }
        public static void UpdatePosition(LatLong position) { commandList.Add("u " + position.ToString()); }
        public static void StartGame(String gameId) { commandList.Add("s " + gameId); }
        public static void GetTargetPos(String gameId) { commandList.Add("t " + gameId); }
        public static void Kill(String gameId) { commandList.Add("k " + gameId); }
        public static void SetName(String name) { commandList.Add("n " + name); }

        public static async void Start(){
            int i = 0;
            while (true)
            {
                if (!RUNNING_SERVER) return;
                if (i % 10==0)
                {
                    await updateCommands();
                }
                if (string.Compare(AppShell.Current.CurrentPage.GetType().Name, "StartGamePage")==0)
                {
                   StartGamePageViewModel viewModel = (StartGamePageViewModel)((StartGamePage)AppShell.Current.CurrentPage).BindingContext;
                    if (getRoom((viewModel).CurrRoom.Id).started == true)
                    {
                        //await Task.Delay(1000);
                        viewModel.toRoomDetails();
                    }
                }
                else if (string.Compare(AppShell.Current.CurrentPage.GetType().Name, "RoomDetailPage") == 0)
                {
                    RoomDetailPageViewModel viewModel = (RoomDetailPageViewModel)((RoomDetailPage)AppShell.Current.CurrentPage).BindingContext;
                    if (getRoom((viewModel).CurrRoom.Id).Winner != null)
                    {
                        await Shell.Current.GoToAsync(nameof(WonGamePage),
                        new Dictionary<string, object>
                        {
                            //get the room we made with the textbox inside of it
                            ["Winner"] = getRoom((viewModel).CurrRoom.Id).Winner.Name + " won the Game"
                        });
                    }
                    else if (getRoom((viewModel).CurrRoom.Id).IsAlive != true)
                    {
                        await Shell.Current.GoToAsync(nameof(DeadPage),
                        new Dictionary<string, object>
                        {
                            ["CurrRoom"] = getRoom((viewModel).CurrRoom.Id)
                        });
                    }
                    
                }
                else if (string.Compare(AppShell.Current.CurrentPage.GetType().Name, "DeadPage") == 0)
                {
                    DeadPageViewModel viewModel = (DeadPageViewModel)((DeadPage)AppShell.Current.CurrentPage).BindingContext;
                    if (getRoom((viewModel).CurrRoom.Id).Winner!=null)
                    {
                        await Shell.Current.GoToAsync(nameof(WonGamePage),
                        new Dictionary<string, object>
                        {
                            //get the room we made with the textbox inside of it
                            ["Winner"] = getRoom((viewModel).CurrRoom.Id).Winner.Name + " won the Game"
                        });
                    }
                }
                processCommands();
                i++;
                await Task.Delay(100); // waits 100 milliseconds between calls
            }
        }

        private async static Task<Task> updateCommands()
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(30));
            Location l = await Geolocation.Default.GetLocationAsync(request);



            LatLong location = new LatLong(l.Latitude, l.Longitude);
            UpdatePosition(location);
            position = location;
            // if (AppShell.Current.CurrentPage. == "meme")
            return Task.Delay(0);
            //TODO ADD HEADING AND ARROW ANGLE CALCULATIONS
        }


        private static void processCommands()
        {
            if (commandList.Count == 0 || c.clientId == null) return;
            String nextCommand = commandList[0];
            // i am well aware this can be more efficient if i pull out the Remove lement line, but for rn, im working on something big, so ill come back to it
            if (nextCommand[0] == 'p') { c.Ping(); commandList.RemoveAt(0); }
            if (nextCommand[0] == 'c') { c.CreateGame(nextCommand.Split()[1]); commandList.RemoveAt(0); } //8e1ace7c-6efc-4804-5d9f-d2ac96505786
            if (nextCommand[0] == 'j') { c.JoinGame(nextCommand.Split()[1]); commandList.RemoveAt(0); }
            if (nextCommand[0] == 'f') { c.FetchGames(nextCommand.Split()[1]); commandList.RemoveAt(0); }
            if (nextCommand[0] == 'u') { c.UpdatePosition(nextCommand.Split()[1]); commandList.RemoveAt(0); }
            if (nextCommand[0] == 's') { c.StartGame(nextCommand.Split()[1]); commandList.RemoveAt(0); }
            if (nextCommand[0] == 't') { c.GetTargetPos(nextCommand.Split()[1]); commandList.RemoveAt(0); }
            if (nextCommand[0] == 'k') { c.Kill(nextCommand.Split()[1]); commandList.RemoveAt(0); }
            if (nextCommand[0] == 'n') { c.SetName(nextCommand.Split()[1]); commandList.RemoveAt(0); }
        }
        public static Room getRoom(String s)
        {
            foreach(Room r in c.gamesJoined.Values)
            {
                if (r.Id.Equals(s))
                {
                    return r;
                }
            }
            return null;
        }
        public static LatLong getTargetPos(String gameId)
        {
            return c.gamesJoined[gameId].target.Position;
        }
    }
}