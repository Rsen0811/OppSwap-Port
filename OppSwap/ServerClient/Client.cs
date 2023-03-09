using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace OppSwap
{
    class Client
    {
        WebSocket ws;
        String clientId;

        public Client()
        {
            ws = new WebSocket("ws://localhost:9090");
            ws.Connect();
        }

        public void Ping()
        {
            string s = @"{""method"": ""ping""}";
            ws.Connect();
            ws.Send(s);
        }
    }
}
