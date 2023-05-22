using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using OppSwap;

namespace SerializedJSONTemplates
{
    //general
	[Serializable]
	public class JPackage
	{
		public string method { get; set; }
	}

    [Serializable]

    // receiving packets
    public class JPGeneral : JPackage 
    {
        public string payload { get; set; } // this is a JSON
    }

    [Serializable]
    public class ConnectPayload : JPackage    
    {
        public String clientId { get; set; }    
        
        public static explicit operator ConnectPayload(JPGeneral incoming)
        {
            ConnectPayload outgoing = JsonConvert.DeserializeObject<ConnectPayload>(incoming.payload);
            outgoing.method = incoming.method;
            return outgoing;
        }
    }

    [Serializable]
    public class StartPayload : JPackage
    {
        public String targetId { get; set; }
        public String targetName { get; set; }
        public String gameId { get; set; }

        public static explicit operator StartPayload(JPGeneral incoming)
        {
            StartPayload outgoing = JsonConvert.DeserializeObject<StartPayload>(incoming.payload);
            outgoing.method = incoming.method;
            return outgoing;
        }
    }

    [Serializable]
    public class JoinPayload : JPackage 
    {
        public String gameName { get; set; }
        public String gameId { get; set; }


        public static explicit operator JoinPayload(JPGeneral incoming)
        {
            JoinPayload outgoing = JsonConvert.DeserializeObject<JoinPayload>(incoming.payload);
            outgoing.method = incoming.method;
            return outgoing;
        }
    }

    [Serializable]
    public class playerJoinPayload : JPackage
    {
        public string gameId { get; set; }
        public string[] clientIds { get; set; }
        public string[] clientNames { get; set; }
        public List<Player> players { get; set; }

        public static explicit operator playerJoinPayload(JPGeneral incoming)
        {
            playerJoinPayload outgoing = JsonConvert.DeserializeObject<playerJoinPayload>(incoming.payload);
            for (int i = 0; i < outgoing.clientIds.Length; i++)
            {
                outgoing.players.Add(new Player(outgoing.clientNames[i], outgoing.clientIds[i]));
            }

            outgoing.method = incoming.method;
            
            return outgoing;
        }
    }

    [Serializable]
    public class GameQueryPackage : JPackage
    {
        public List<Room> rooms { get; set; }
        public String[] names { get; set; }
        public String[] ids { get; set; }

        public static explicit operator GameQueryPackage(JPGeneral incoming)
        {
            GameQueryPackage outgoing = JsonConvert.DeserializeObject<GameQueryPackage>(incoming.payload);
            outgoing.rooms = new List<Room>();
            for (int i = 0; i < outgoing.names.Length; i++)
            {
                outgoing.rooms.Add(new Room(outgoing.names[i], outgoing.ids[i]));
            }
            outgoing.method = incoming.method;
            return outgoing;
        }
    }

    [Serializable]
    public class TargetPosPackage : JPackage
    {
        public String targetPostion { get; set; }
        public String gameId { get; set; }

        public static explicit operator TargetPosPackage(JPGeneral incoming)
        {
            TargetPosPackage outgoing = JsonConvert.DeserializeObject<TargetPosPackage>(incoming.payload);
            outgoing.method = incoming.method;
            return outgoing;
        }
    }
    
    //TODO ask raj what is diif between payload vs packages???
    //TODO add the nicknames for the Target
    [Serializable]
    public class TargetPackage : JPackage
    {
        public String targetId { get; set; }
        public String gameId { get; set; }
        public String targetName { get; set; }

        public static explicit operator TargetPackage(JPGeneral incoming)
        {
            TargetPackage outgoing = JsonConvert.DeserializeObject<TargetPackage>(incoming.payload);
            outgoing.method = incoming.method;
            return outgoing;
        }
    }

    //sending packets

    // send single string
    // for updating name or position
    [Serializable]
    public class String1Payload : JPackage
    {
        public string gameId { get; set; }
        public string clientId { get; set; }
        public string value { get; set; }
    }

}
