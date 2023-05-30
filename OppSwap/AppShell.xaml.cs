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
            Routing.RegisterRoute(nameof(CreatePage), typeof(CreatePage));
            Routing.RegisterRoute(nameof(FetchedGamesPage), typeof(FetchedGamesPage));
            Routing.RegisterRoute(nameof(StartGamePage), typeof(StartGamePage));
            Routing.RegisterRoute(nameof(DeadPage), typeof(DeadPage));
            Routing.RegisterRoute(nameof(WonGamePage), typeof(WonGamePage));
        }
    }
}   