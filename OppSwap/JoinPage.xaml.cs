using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OppSwap.ViewModels;
namespace OppSwap
{
    public partial class JoinPage : ContentPage
    {
        //Client c = new Client();
        int count = 0;

        public JoinPage(JoinPageViewModel vm)
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
        async void JoinButton_Clicked(System.Object sender, System.EventArgs e)
        {

            //TODO  find out why does this only work with the.c no work without the.c
            ClientInterconnect.FetchGames("");
            await Task.Delay(1000);

            await Shell.Current.GoToAsync(nameof(FetchedGamesPage),
            new Dictionary<string, object>
            {
                //get the room we made with the textbox inside of it
                //TODO Changed Joined Games to Fetched Games
                ["FetchedGames"] = ClientInterconnect.c.fetchedRooms
            }) ;
         }



    }
}