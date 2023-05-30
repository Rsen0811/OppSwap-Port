
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

        private void OnCounterClicked(object sender, EventArgs e)
        {
            //c.Ping();
        }
    }
}