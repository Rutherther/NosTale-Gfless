using Newtonsoft.Json;

namespace NosTaleGfless.Pipes
{
    public class ResultMessage<T> : IdMessage
    {
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}