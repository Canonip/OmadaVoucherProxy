using Newtonsoft.Json;
using OmadaAPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmadaAPI.Model.DTO
{
    public class BaseResponse : BaseResponse<object> { }
    public class BaseResponse<T>
    {
        [JsonIgnore]
        public bool IsSuccessStatusCode => StatusCode == 0;
        public void EnsureSuccessStatusCode()
        {
            if (!IsSuccessStatusCode)
                throw new OmadaResponseException(Message, StatusCode);
        }
        [JsonProperty("errorCode")]
        public int StatusCode { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}
