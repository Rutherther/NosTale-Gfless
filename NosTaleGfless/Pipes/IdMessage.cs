using Newtonsoft.Json;

namespace NosTaleGfless.Pipes
{
    public class IdMessage
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("jsonrpc")]
        public double Jsonrpc { get; set; } = 2.0;
    }
}