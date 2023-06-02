
using OppSwap.ViewModels;
namespace OppSwap
{
    public partial class DeadPage : ContentPage
    {
        //Client c = new Client();
        int count = 0;

        public DeadPage(DeadPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            vm.Updates();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            //c.Ping();
        }
    }
}