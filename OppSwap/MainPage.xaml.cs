using OppSwap.ViewModels;
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
            //ClientInterconnect.Ping();
            //ClientInterconnect.c.Reconnect("a3977364-2bdd-413b-c367-b9ed19c764b5");
            ClientInterconnect.JoinGame("13d094ae-c7ac-4d38-675d-cfb0dd935842");
            ClientInterconnect.StartGame("13d094ae-c7ac-4d38-675d-cfb0dd935842");
            ClientInterconnect.Kill("13d094ae-c7ac-4d38-675d-cfb0dd935842");
            //ClientInterconnect.FetchGames("for");
            //ClientInterconnect.UpdatePosition(new LatLong(123, 345));
            //ClientInterconnect.c.GetTargetPos("8070b607-9e52-402e-c1d2-d6656009c6a8");
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void NextPage(object sender, EventArgs e) {

            await Shell.Current.GoToAsync(nameof(JoinPage),
           new Dictionary<string, object>
           {
               //get the room we made with the textbox inside of it
               ["JoinedGames"] = ClientInterconnect.c.gamesJoined.Values
           });
        }
    }
}