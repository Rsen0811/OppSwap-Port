﻿using OppSwap.ViewModels;
namespace OppSwap
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            ClientInterconnect.Ping();
            ClientInterconnect.FetchGames("for");
            //ClientInterconnect.UpdatePosition(new LatLong(123, 345));
            ClientInterconnect.c.TempGetPos("8070b607-9e52-402e-c1d2-d6656009c6a8");
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void NextPage(object sender, EventArgs e) {
            await Shell.Current.GoToAsync(nameof(JoinPage));
        }
    }
}