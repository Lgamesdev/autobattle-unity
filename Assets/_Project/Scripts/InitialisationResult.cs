using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LGamesDev
{
    public class InitialisationResult
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public InitialisationStage Stage;
        
        public string Value;
    }
}