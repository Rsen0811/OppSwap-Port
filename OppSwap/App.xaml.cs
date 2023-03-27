namespace OppSwap
{
    public partial class App : Application
    {
        public App()
        {            
            ClientInterconnect.Start();
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}