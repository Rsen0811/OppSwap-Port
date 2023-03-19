using System;
using System.CodeDom;
using System.Net.NetworkInformation;

namespace OppSwap
{
	public class ClientInterconnect
	{
		public static readonly Client c = new Client();
		public ClientInterconnect()
		{
			
		}

        public static void Ping() { c.Ping(); }
        public static void CreateGame(String name) { c.CreateGame(name); }

    }
}