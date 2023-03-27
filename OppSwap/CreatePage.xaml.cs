
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

        private void OnCounterClicked(object sender, EventArgs e)
        {
            //c.Ping();
        }
    }
}