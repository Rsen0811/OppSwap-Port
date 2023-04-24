﻿using System;
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
        public Dictionary<String,Room> gamesJoined;
        public List<Room> fetchedRooms;
        public Client()
        {
            ws = new WebSocket("ws://localhost:9992");//ws://water-cautious-barge.glitch.me");
            ws.Connect();
            ws.OnMessage += Ws_OnMessage;

            gamesJoined = new Dictionary<String,Room>();
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
        public void StartGame(String gameId)
        {
            ws.Send(JsonConvert.SerializeObject(new
            {
                method = "startGame",
                clientId = clientId,
                gameId = gameId
            }));
        }

        public void GetTargetPos(String gameId)
        {
            ws.Send(JsonConvert.SerializeObject(new
            {
                method = "getTargetPosition",
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
                //gamesJoined.Add(new Room(p.gameName, p.gameId));
                gamesJoined.Add(p.gameId, new Room(p.gameName, p.gameId));
            }
            if (packet.method.Equals("playerJoinUpdate"))
            {
                playerJoinPayload p = (playerJoinPayload)packet;
                foreach (Room r in gamesJoined.Values)
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
                gamesJoined[p.gameId].target =new Target(p.targetId);
                //TODO call getPos here
            }
        }
    }
}
