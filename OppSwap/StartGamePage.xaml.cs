using OppSwap.ViewModels;
using System;
using Microsoft.Maui.Controls.Xaml.Diagnostics;
using Microsoft.Maui.Controls.Xaml.Internals;
using Microsoft.Maui.Graphics;
//using Android.Graphics;

namespace OppSwap
{
    public partial class StartGamePage : ContentPage
    {
        //Client c = new Client();
        //int count = 0;
        //String roomID;

        //ICanvas canvas;

        public StartGamePage(StartGamePageViewModel vm)
        {
            this.InitializeComponent();
            BindingContext = vm;
            vm.Updates();
        }
    }

}