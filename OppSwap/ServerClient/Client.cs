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
            //make a websockett with on IP of localhost with a port of 9092
            ws = new WebSocket("ws://localhost:9092");//ws://water-cautious-barge.glitch.me");
            //why not asynch connect whats the difference?
            ws.Connect();
            // when you recieve a message call the method on message
            //can you make it call multiple methods if so in what order does it call them?
            ws.OnMessage += Ws_OnMessage;
            //EX:
            //ws.OnMessage += sum;

            JPGeneral p = new JPGeneral
            {
                message = "connect",
                payload = "32000"
            };
            //encoding the message connect as a method?
            //why did u call it method?

            ws.Send(JsonConvert.SerializeObject(p));


        }
        //where is this called?
        public void Ping()
        {
            //ws.Connect();

            JPackage p = new JPackage
            {
                message = "ping"
            };

            ws.Send(JsonConvert.SerializeObject(p));
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            //why not just
            //JPackage packet = JsonConvert.DeserializeObject<JPackage>(e.Data);
            JPGeneral packet = JsonConvert.DeserializeObject<JPGeneral>(e.Data);
            if (packet.message.Equals("connect"))
            {
                //ConnectPayload p = JsonConvert.DeserializeObject<ConnectPayload>(packet.payload);
                clientId = packet.payload; //((ConnectPayload)packet).clientId;
            }
        }
    }
}
