using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using SerializedJSONTemplates;
using Newtonsoft.Json;

namespace OppSwap
{
    class Client
    {
        WebSocket ws;
        String clientId;

        public Client()
        {
            ws = new WebSocket("ws://localhost:9092");//ws://water-cautious-barge.glitch.me");
            ws.Connect();
            ws.OnMessage += Ws_OnMessage;

            JPackage p = new JPackage
            {
                method = "connect"
            };

            ws.Send(JsonConvert.SerializeObject(p));


        }

        public void Ping()
        {
            //ws.Connect();

            JPackage p = new JPackage
            {
                method = "ping"
            };

            ws.Send(JsonConvert.SerializeObject(p));
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            JPGeneral packet = JsonConvert.DeserializeObject<JPGeneral>(e.Data);
            if (packet.method.Equals("connect"))
            {
                //ConnectPayload p = JsonConvert.DeserializeObject<ConnectPayload>(packet.payload);
                clientId = ((ConnectPayload)packet).clientId;
            }
        }
    }
}
