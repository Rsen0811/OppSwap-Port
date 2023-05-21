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

        ICanvas canvas;

        public RoomDetailPage(RoomDetailPageViewModel vm)
        {
            this.InitializeComponent();
            BindingContext = vm;
            vm.KillButtonVisible();
        }
        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
        }




    }

}