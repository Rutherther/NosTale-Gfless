using Newtonsoft.Json;

namespace NosTaleGfless.Pipes
{
    public class ParamsMessage : IdMessage
    {
        [JsonProperty("method")]
        public string Method { get; set; }
        
        [JsonProperty("params")]
        public SessionParams Params { get; set; }
    }
}