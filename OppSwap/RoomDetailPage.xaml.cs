using OppSwap.ViewModels;
namespace OppSwap
{
    public partial class RoomDetailPage : ContentPage
    {
        //Client c = new Client();
        int count = 0;
        String roomID;

        public RoomDetailPage(RoomDetailPageViewModel vm)
        {
            this.InitializeComponent();
            BindingContext = vm;
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