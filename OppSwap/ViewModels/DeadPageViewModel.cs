using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace OppSwap.ViewModels
{
    [QueryProperty(nameof(CurrRoom)/*The name of the property in ViewModel*/, nameof(CurrRoom)/*The name of the property when you switch pages*/)]

    public partial class DeadPageViewModel : ObservableObject
	{
        [ObservableProperty]
        Room currRoom;

        [ObservableProperty]
        List<string> deadPlayers;

        [ObservableProperty]
        List<string> alivePlayers;

        public DeadPageViewModel() {

		}

		[ObservableProperty]
		string gameCode;

		[RelayCommand]
		void createRoom()
		{
			if (string.IsNullOrWhiteSpace(GameCode)) {
				return;
			}
			ClientInterconnect.CreateGame(GameCode);
            GameCode = "";
		}
        [RelayCommand]
        public async Task Updates()
        {
            await Task.Delay(1000);
            while (true)
            {
                if (CurrRoom != null)
                {
                    if (!CurrRoom.Equals(ClientInterconnect.c.gamesJoined[CurrRoom.Id]))
                        CurrRoom = ClientInterconnect.c.gamesJoined[CurrRoom.Id];
                    if (CurrRoom.players != null)
                    {
                        if (DeadPlayers == null || !CompareArrays(DeadPlayers, DeadPlayerToName(ClientInterconnect.c.gamesJoined[CurrRoom.Id].players)))
                        {
                            DeadPlayers = DeadPlayerToName(CurrRoom.players);
                        }
                        if (AlivePlayers == null || !CompareArrays(AlivePlayers, AlivePlayerToName(ClientInterconnect.c.gamesJoined[CurrRoom.Id].players)))
                        {
                            AlivePlayers = AlivePlayerToName(CurrRoom.players);
                        }

                    }
                }
                await Task.Delay(100);//wait 100 miliseconds
            }
        }
        private bool CompareArrays(List<string> arr1, List<string> arr2)
        {
            if (arr1.Count != arr2.Count)
            {
                return false;
            }
            for (int i = 0; i < arr1.Count; i++)
            {
                if (string.Compare(arr1[i], arr2[i]) != 0)
                {
                    return false;
                }
            }
            return true;
        }
        private List<string> DeadPlayerToName(List<Player> players)
        {
            List<string> temp = new List<string>();
            foreach (Player p in players)
            {
                if (!p.IsAlive&&!ClientInterconnect.c.clientId.Equals(p.Id))
                {
                    temp.Add(p.Name);
                }
            }
            return temp;
        }
        private List<string> AlivePlayerToName(List<Player> players)
        {
            List<string> temp = new List<string>();
            foreach (Player p in players)
            {
                if (p.IsAlive&&!ClientInterconnect.c.clientId.Equals(p.Id))
                {
                    temp.Add(p.Name);
                }
            }
            return temp;
        }
    }
}

