using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

namespace SerializedJSONTemplates
{
    //general
	[Serializable]
	public class JPackage
	{
		public string message { get; set; }
	}

    [Serializable]

    // receiving packets
    public class JPGeneral : JPackage 
    {
        //In the format of a json 
        public string payload { get; set; }
    }

    [Serializable]
    public class ConnectPayload : JPackage
    {
        public String clientId { get; set; }
        
        public static explicit operator ConnectPayload(JPGeneral incoming)
        {
            ConnectPayload outgoing = JsonConvert.DeserializeObject<ConnectPayload>(incoming.payload);
            outgoing.message = incoming.message;
            return outgoing;
        }
    }

    //sending packets
}
