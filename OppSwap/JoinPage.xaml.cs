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
        private async void NextPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(RoomDetailPage));
        }

        void JoinButton_Clicked(System.Object sender, System.EventArgs e)
        {
        }

        
    }
}