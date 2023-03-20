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
        private async void NextPage(object sender, EventArgs e) {
            await Shell.Current.GoToAsync(nameof(JoinPage));
        }
    }
}