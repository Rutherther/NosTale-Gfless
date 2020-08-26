using Newtonsoft.Json;

namespace NostaleAuth.Models
{
    public class AuthRequest
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
