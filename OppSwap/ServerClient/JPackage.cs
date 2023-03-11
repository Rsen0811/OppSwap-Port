using Newtonsoft.Json;
using System;

namespace SerializedJSONTemplates
{
	[Serializable]
	public class JPackage
	{
		public string method { get; set; }
	}

    [Serializable]

    public class JPGeneral : JPackage 
    {
        public string payload { get; set; }
    }

    [Serializable]
    public class ConnectPayload : JPackage
    {
        public String clientId { get; set; }
        
        public static explicit operator ConnectPayload(JPGeneral incoming)
        {
            String jsonString = $"{{\"method\":\"{incoming.method}\",{incoming.payload.Substring(1, incoming.payload.Length - 2)}}}";  
            ConnectPayload outgoing = JsonConvert.DeserializeObject<ConnectPayload>(jsonString);

            return outgoing;
        }
    }
}
