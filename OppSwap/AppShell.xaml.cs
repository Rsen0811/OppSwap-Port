namespace OppSwap
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(JoinPage), typeof(JoinPage));
            Routing.RegisterRoute(nameof(CreateGamePage), typeof(CreateGamePage));
            Routing.RegisterRoute(nameof(RoomDetailPage), typeof(RoomDetailPage));
        }
    }
}