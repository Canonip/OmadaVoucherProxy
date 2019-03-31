using Newtonsoft.Json;
using System;

namespace OmadaAPI.Model.DTO
{

    public class LaunchStatusResult
    {
        [JsonProperty("serverKey")]
        public string CloudControllerId { get; set; }
        [JsonProperty("status")]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public LaunchStatus Status => Enum.IsDefined(typeof(LaunchStatus), StatusCode) ? (LaunchStatus)StatusCode : LaunchStatus.Unknown;
    }

}
