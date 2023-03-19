using System;
using System.CodeDom;
using System.Net.NetworkInformation;
using System.Threading;

namespace OppSwap
{
    public static class ClientInterconnect
    {
        public static readonly Client c = new Client();

        private static List<String> commandList = new List<string>();

        public static void Ping() { commandList.Add("p"); }
        public static void CreateGame(String name) { commandList.Add("c " + name); }

        public static async void Start()
        {
            while (true)
            {
                updateCommands();
                await Task.Delay(100); // waits 100 ms between calls
            }
        }

        public static void updateCommands()
        {
            if (commandList.Count == 0 || c.clientId == null) return;

            String nextCommand = commandList[0];

            if (nextCommand[0] == 'p') { c.Ping(); commandList.RemoveAt(0); }
            if (nextCommand[0] == 'c') { c.CreateGame(nextCommand.Split()[1]); commandList.RemoveAt(0); }
        }
    }
}