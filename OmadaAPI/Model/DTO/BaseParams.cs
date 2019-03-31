using Newtonsoft.Json;

namespace OmadaAPI.Model.DTO
{
    public abstract class BaseParams
    {
        [JsonIgnore]
        public string Method { get; protected set; }
        /// <summary>
        /// Creates the Json Payload to send to the Server
        /// </summary>
        /// <returns>Json Payload</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(new RequestPayload { Method = Method, Params = this },
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
