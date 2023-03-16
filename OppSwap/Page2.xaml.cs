using OppSwap.ViewModels;
namespace OppSwap
{
    public partial class Page2 : ContentPage
    {
        //Client c = new Client();
        int count = 0;

        public Page2(Page2ViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            //c.Ping();
        }

        void JoinButton_Clicked(System.Object sender, System.EventArgs e)
        {
        }
    }
}