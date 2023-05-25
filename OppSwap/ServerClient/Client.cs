using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using SerializedJSONTemplates;
using Newtonsoft.Json;
using System.Xml.Linq;
//using static Android.Icu.Text.Transliterator;

namespace OppSwap
{
    public class Client
    {
        private WebSocket ws;
        public String clientId;
        public Dictionary<string,Room> gamesJoined;
        public List<Room> fetchedRooms;
        public List<String> errorMessages;
        public Client()
        {
            ws = new WebSocket("ws://localhost:9992");//ws://water-cautious-barge.glitch.me");
            ws.Connect();
            ws.OnMessage += Ws_OnMessage;

            gamesJoined = new Dictionary<String,Room>();
            fetchedRooms = new List<Room>();
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

        private bool ValidKill()
        {
            return true;
        }

        public void Kill(String gameId)
        {
            if (!ValidKill()) return;
            
            ws.Connect();
            String1Payload p = new String1Payload
            {
                method = "kill",
                clientId = clientId,
                gameId = gameId
            };
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
        public void SetName(String name) 
        {
            ws.Connect();
            ws.Send(JsonConvert.SerializeObject(new
            {
                method = "setName",
                clientId = clientId,
                name = name
            }));
        }
        
        public void UpdatePosition(LatLong position)
        {
            UpdatePosition(position.ToString());
        }

        public void UpdatePosition(String position) {
            ws.Connect();
            ws.Send(JsonConvert.SerializeObject(new {
                method = "updatePosition",
                clientId = clientId,
                ///gamesJoined = gamesJoined.ToArray(), // not needed because we no longer store position in the rooms themselves
                position = position
            }));
        }
        public void StartGame(String gameId)
        {
            ws.Connect();
            ws.Send(JsonConvert.SerializeObject(new
            {
                method = "startGame",
                clientId = clientId,
                gameId = gameId
            }));
        }

        public void GetTargetPos(String gameId)
        {
            ws.Connect();
            ws.Send(JsonConvert.SerializeObject(new
            {
                method = "getTargetPosition",
                clientId = clientId,
                gameId = gameId
            }));
        }

        public void Reconnect(String oldGuid) // must have guid from previous state
        {
            ws.Connect();
            ws.Send(JsonConvert.SerializeObject(new
            {
                method = "reconnect",
                clientId = clientId,
                oldId = oldGuid
            }));
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e) //gotta make these things their own methods but not rn
        {
            //TODO these should also all be else IFS
            JPGeneral packet = JsonConvert.DeserializeObject<JPGeneral>(e.Data);
            if (packet.method.Equals("connect"))
            {
                //ConnectPayload p = JsonConvert.DeserializeObject<ConnectPayload>(packet.payload);
                clientId = ((ConnectPayload)packet).clientId;
            }
            if (packet.method.Equals("forceJoin"))
            {
                JoinPayload p = (JoinPayload)packet;
                //gamesJoined.Add(new Room(p.gameName, p.gameId));
                gamesJoined.Add(p.gameId, new Room(p.gameName, p.gameId));
            }
            if (packet.method.Equals("playerJoinUpdate"))
            {
                playerJoinPayload p = (playerJoinPayload)packet;
                Room r = gamesJoined[p.gameId];
                r.players = p.players;
            }
            if (packet.method.Equals("fetchGames"))
            {
                //this isnt working or smthn
                //TODO ASK RAJ HOW TO FIX THIS.
                GameQueryPackage p = (GameQueryPackage)packet;
                fetchedRooms = p.rooms;
            }

            if (packet.method.Equals("getPosition"))
            {
                TargetPosPackage p = (TargetPosPackage)packet;
                gamesJoined[p.gameId].target.Position = new LatLong(p.targetPostion);
            }

            if (packet.method.Equals("gameStarted"))
            {
                StartPayload p = (StartPayload)packet;
                //TODO eventually we can look into transferring the nickname instead of targetID, for now use target ID as a replacement Nick when displaying target name
                //target initially has a position of 0,0
                gamesJoined[p.gameId].target = new Target(p.targetId, p.targetName);
                //TODO call getPos here
            }
            if(packet.method.Equals("serverMessage"))
            {
                ServerMessage p = (ServerMessage)packet;
                //AppShell.Current.CurrentPage.DisplayAlert("Server Message", p.message, "Accept");
                //MainPage.DisplayAlert("Server Message", p.message, "Accept");
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Application.Current.MainPage.DisplayAlert("Server Message", p.message, "Accept");
                    AppShell.Current.CurrentPage.DisplayAlert("Server Message", p.message, "Accept");
                });
            }
            if (packet.method.Equals("newTarget"))
            {
                TargetPackage p = (TargetPackage)packet;
                gamesJoined[p.gameId].target = new Target(p.targetId, p.targetName);
            }

            if (packet.method.Equals("nickName"))
            {
                NickNamePackage p = (NickNamePackage)packet;
                foreach(string game in p.gamesJoined)
                {
                    Room cur = gamesJoined[game];
                    if (cur != null)
                    {
                        if (cur.target.Id == p.clientId)
                        {
                            cur.target.Name = p.name;
                        }
                        foreach(Player player in cur.players)
                        {
                            if (player.Id == p.clientId)
                            {
                                player.Name = p.name;
                                break;
                            }
                        }
                    }
                } 
            }
        }
    }
}
