using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using SerializedJSONTemplates;
using Newtonsoft.Json;
using static Java.Util.Jar.Attributes;

namespace OppSwap
{
    class Client
    {
        private WebSocket ws;
        public String clientId;
        public (String, String)[] gamesJoined; // name, id
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

            JPackage p = new JPackage { method = "ping" };
            ws.Send(JsonConvert.SerializeObject(p));
        }

        public void createGame(String name) 
        { // creator gets sent a special payload to automatically join game
            String1Payload p = new String1Payload
            {
                method = "createNewGame",
                clientId = clientId,
                gameId = null,
                value = name
            };
            ws.Send(JsonConvert.SerializeObject(p));
        }
        public (String, String)[] FetchGames(String query)
        {
            return null; // implement later fetch list of games that exist with partial matches
        }
        public void JoinGame(String gameId) 
        {
            String1Payload p = new String1Payload
            {
                method = "joinGame",
                clientId = clientId,
                gameId = gameId,
                value = null
            };
        }
        public void SetName(String name) { }
        public void UpdatePosition(String pos) { }
        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            JPGeneral packet = JsonConvert.DeserializeObject<JPGeneral>(e.Data);
            if (packet.method.Equals("connect"))
            {
                //ConnectPayload p = JsonConvert.DeserializeObject<ConnectPayload>(packet.payload);
                clientId = ((ConnectPayload)packet).clientId;
            }
            if (packet.method.Equals("forceJoin"))
            {
                JoinPayload p = (JoinPayload)packet;
                gamesJoined.Append((p.gameName, p.gameId));
            }
        }
    }
}
