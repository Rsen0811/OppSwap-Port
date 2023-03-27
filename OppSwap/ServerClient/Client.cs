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
    public class Client
    {
        private WebSocket ws;
        public String clientId;
        public List<Room> gamesJoined;
        public List<Room> fetchedRooms;
        public String enemyPos = ""; //#=============== fakecode

        public Client()
        {
            ws = new WebSocket("ws://localhost:9992");//ws://water-cautious-barge.glitch.me");
            ws.Connect();
            ws.OnMessage += Ws_OnMessage;

            gamesJoined = new List<Room>();
            //   outdated code from a more civilized age
            JPackage p = new JPackage
            {
                method = "connect"
            };
            
            // currently not having save data, but eventually i want to store the client id between app closes, and send a connect request that i send the guid
            ws.Send(JsonConvert.SerializeObject(p));
        }

        public void Ping()
        {
            ws.Connect();

            JPackage p = new JPackage { method = "ping" };
            ws.Send(JsonConvert.SerializeObject(p));
        }

        public void CreateGame(String name) 
        { // creator gets sent a special payload to automatically join game
            ws.Connect();
            String1Payload p = new String1Payload
            {
                method = "createNewGame",
                clientId = clientId,
                gameId = null,
                value = name
            };
            ws.Send(JsonConvert.SerializeObject(p));
        }

        public void FetchGames(String query)
        {
            ws.Connect();
            ws.Send(JsonConvert.SerializeObject(new { method = "fetchGames", query = query }));
        }
        public void JoinGame(String gameId) 
        {
            ws.Connect();
            String1Payload p = new String1Payload
            {
                method = "joinGame",
                clientId = clientId,
                gameId = gameId,
                value = null
            };
            ws.Send(JsonConvert.SerializeObject(p));
        }
        public void SetName(String name) { }
        
        public void UpdatePosition(LatLong position)
        {
            UpdatePosition(position.ToString());
        }

        public void UpdatePosition(String position) {
            ws.Send(JsonConvert.SerializeObject(new {
                method = "updatePosition",
                clientId = clientId,
                gamesJoined = gamesJoined.ToArray(),
                position = position
            }));
        }

        public void TempGetPos(String gameId) //#=============== fakecode
        {
            ws.Send(JsonConvert.SerializeObject(new
            {
                method = "TP",
                clientId = clientId,
                gameId = gameId
            }));
        }
        private void Ws_OnMessage(object sender, MessageEventArgs e) //gotta make these things their own methods but not rn
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
                gamesJoined.Add(new Room(p.gameName, p.gameId));
                //gamesJoined.Append((p.gameName, p.gameId)); // change because this no longer makes sense
            }
            if (packet.method.Equals("playerJoinUpdate"))
            {
                playerJoinPayload p = (playerJoinPayload)packet;
                foreach (Room r in gamesJoined)
                {
                    if (r.Id.Equals(p.gameId))
                    {
                        r.tempholderwhileplayersdonthavenamesonserver = p.clients;
                        return;
                    }
                }
            }
            if (packet.method.Equals("fetchGames"))
            {
                GameQueryPackage p = (GameQueryPackage)packet;
                fetchedRooms = p.rooms;
            }

            if (packet.method.Equals("TP"))
            {
                enemyPos = packet.payload;
            }
        }
    }
}
