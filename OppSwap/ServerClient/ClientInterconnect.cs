using System;
using System.CodeDom;
using System.Net.NetworkInformation;
using System.Threading;

namespace OppSwap
{
    public static class ClientInterconnect
    {
#if DISABLE_SERVER
        public static readonly bool RUNNING_SERVER = false; // turn this to true if running, false if not
#else
        public static readonly bool RUNNING_SERVER = true; // turn this to true if running, false if not
#endif
        public static readonly Client c = (RUNNING_SERVER ? new Client() : null);

        private static List<String> commandList = new List<string>();

        public static void Ping() { commandList.Add("p"); }
        public static void CreateGame(String name) { commandList.Add("c " + name); }

        public static void JoinGame(String gameId) { commandList.Add("j " + gameId); }
        public static void FetchGames(String query) { commandList.Add("s " + query); }

        public static async void Start()
        {
            while (true)
            {
                if (!RUNNING_SERVER) return;
                updateCommands();
                await Task.Delay(100); // waits 100 ms between calls
            }
        }

        public static void updateCommands()
        {
            if (commandList.Count == 0 || c.clientId == null) return;

            String nextCommand = commandList[0];
            // i am well aware this can be more efficient if i pull out the Remove lement line, but for rn, im working on something big, so ill come back to it
            if (nextCommand[0] == 'p') { c.Ping(); commandList.RemoveAt(0); }
            if (nextCommand[0] == 'c') { c.CreateGame(nextCommand.Split()[1]); commandList.RemoveAt(0); } //8e1ace7c-6efc-4804-5d9f-d2ac96505786
            if (nextCommand[0] == 'j') { c.JoinGame(nextCommand.Split()[1]); commandList.RemoveAt(0); }
            if (nextCommand[0] == 's') { c.FetchGames(nextCommand.Split()[1]); commandList.RemoveAt(0); }

        }
    }
}