using OppSwap.ViewModels;
using System;
using Microsoft.Maui.Controls.Xaml.Diagnostics;
using Microsoft.Maui.Controls.Xaml.Internals;

namespace OppSwap
{
    public partial class RoomDetailPage : ContentPage
    {
        //Client c = new Client();
        //int count = 0;
        //String roomID;

        public RoomDetailPage(RoomDetailPageViewModel vm)
        {
            this.InitializeComponent();
            BindingContext = vm;
        }
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
        }

    }

}