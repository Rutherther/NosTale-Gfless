using Newtonsoft.Json;

namespace NostaleAuth.Models
{
    public sealed class SessionRequest
    {
        [JsonProperty("platformGameAccountId")]
        public string PlatformGameAccountId { get; set; }
    }
}
