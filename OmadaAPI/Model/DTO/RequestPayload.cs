using Newtonsoft.Json;

namespace OmadaAPI.Model.DTO
{
    internal class RequestPayload
    {
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("params")]
        public object Params { get; set; }
    }
}
