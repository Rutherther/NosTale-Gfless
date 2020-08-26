using Newtonsoft.Json;

namespace NosTaleGfless.Pipes
{
    public class SessionParams
    {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
    }
}