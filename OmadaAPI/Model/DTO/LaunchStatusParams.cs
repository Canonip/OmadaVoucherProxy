using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmadaAPI.Model.DTO
{
    public class LaunchStatusParams:BaseParams
    {
        [JsonProperty("deviceId")]
        public string CloudControllerId { get; set; }
        public LaunchStatusParams()
        {
            Method = "getLaunchStatus";
        }
    }
}
