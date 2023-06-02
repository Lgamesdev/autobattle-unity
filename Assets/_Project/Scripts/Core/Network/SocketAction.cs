using Newtonsoft.Json;

namespace LGamesDev.Core
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SocketMessage
    {
        [JsonProperty("action")]
        public string Action;
        
        [JsonProperty("channel")]
        public string Channel;
        
        [JsonProperty("username")]
        public string Username;
        
        [JsonProperty("content")]
        public string Content;
        
        [JsonProperty("type")]
        public string Type;
        
        public override string ToString()
        {
            return "SocketMessage : [ \n" +
                       "Action : " + Action + "\n" +
                       "Channel : " + Channel + "\n" +
                       "Username : " + Username + "\n" +
                       "Content : " + Content + "\n" +
                       "Type : " + Type + "\n" +
                   "]";
        }
    }
}