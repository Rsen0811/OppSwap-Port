
using OppSwap.ViewModels;
namespace OppSwap
{
    public partial class CreatePage : ContentPage
    {
        //Client c = new Client();
        int count = 0;

        public CreatePage(CreatePageViewModel vm)
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

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
        }
    }
}