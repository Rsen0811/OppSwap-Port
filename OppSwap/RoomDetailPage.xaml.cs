﻿using OppSwap.ViewModels;
using Microsoft.Maui.Graphics;
//using Android.Graphics;

namespace OppSwap
{
    public partial class RoomDetailPage : ContentPage
    {
        //Client c = new Client();
        int count = 0;
        String roomID;
        ICanvas canvas;

        public RoomDetailPage(RoomDetailPageViewModel vm)
        {
            this.InitializeComponent();
            BindingContext = vm;
        }

        private async void NextPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(JoinPage));
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            //c.Ping();
            count++;


            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}