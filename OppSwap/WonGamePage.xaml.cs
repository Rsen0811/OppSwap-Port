
using OppSwap.ViewModels;
namespace OppSwap
{
    public partial class WonGamePage : ContentPage
    {
        //Client c = new Client();
        int count = 0;

        public WonGamePage(WonGamePageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
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