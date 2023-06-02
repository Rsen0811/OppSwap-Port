using OppSwap.ViewModels;
using System;
using Microsoft.Maui.Controls.Xaml.Diagnostics;
using Microsoft.Maui.Controls.Xaml.Internals;
using Microsoft.Maui.Graphics;
//using Android.Graphics;

namespace OppSwap
{
    public partial class RoomDetailPage : ContentPage
    {
        //Client c = new Client();
        //int count = 0;
        //String roomID;

        //ICanvas canvas;

        public RoomDetailPage(RoomDetailPageViewModel vm)
        {
            this.InitializeComponent();
            BindingContext = vm;
            vm.Update();
        }
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {

                await Shell.Current.GoToAsync(nameof(JoinPage),
                 new Dictionary<string, object>
                 {
                     //get the room we made with the textbox inside of it
                     ["JoinedGames"] = ClientInterconnect.c.gamesJoined
                 });
            
        }

    }

}