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
        
        private static async Task gameJoined(String s)
        {
            while (true)
            {
                await Task.Delay(100);
                foreach (Room r in ClientInterconnect.c.gamesJoined)
                {
                    if (r.Id.Equals(s))
                    {
                        return;
                    }
                }
            }
        }

        private async void NextPageJoin(object sender, EventArgs e)
        {
            // await Shell.Current.GoToAsync(nameof(RoomDetailPage));
            await Shell.Current.GoToAsync(nameof(RoomDetailPage));
        }
        void JoinButton_Clicked(System.Object sender, System.EventArgs e)
        {
        }


    }
}