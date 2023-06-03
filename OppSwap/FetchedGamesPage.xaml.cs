using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OppSwap.ViewModels;
namespace OppSwap
{
    public partial class FetchedGamesPage : ContentPage
    {
        //Client c = new Client();
        int count = 0;

        public FetchedGamesPage(FetchedGamesPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private void CreateButton_Clicked(object sender, EventArgs e)
        {
        }


        private async void GoToCreatePage(object sender, EventArgs e)
        {
            // await Shell.Current.GoToAsync(nameof(RoomDetailPage));
            await Shell.Current.GoToAsync(nameof(CreatePage));
        }
        void JoinButton_Clicked(System.Object sender, System.EventArgs e)
        {
        }
        private async void OnCounterClicked(object sender, EventArgs e)
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