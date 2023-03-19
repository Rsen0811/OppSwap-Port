using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

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
        public string[] clients { get; set; }

        public static explicit operator playerJoinPayload(JPGeneral incoming)
        {
            playerJoinPayload outgoing = JsonConvert.DeserializeObject<playerJoinPayload>(incoming.payload);
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
