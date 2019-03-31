using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OmadaAPI.Model.DTO
{
    public class LoginResult
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
