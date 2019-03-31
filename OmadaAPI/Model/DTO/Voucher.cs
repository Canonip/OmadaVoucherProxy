using Newtonsoft.Json;

namespace OmadaAPI.Model.DTO
{
    public class Voucher
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("codeLength")]
        public int? CodeLength { get; set; }
        [JsonProperty("codeAlias")]
        public string CodeAlias { get; set; }
        [JsonProperty("createdTime")]
        public int CreatedTime { get; set; }
        [JsonProperty("adminName")]
        public string AdminName { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("quota")]
        public int Quota { get; set; }
        [JsonProperty("used")]
        public int Used { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("startTime")]
        public long? StartTime { get; set; }
        [JsonProperty("endTime")]
        public long? EndTime { get; set; }
        [JsonProperty("valid")]
        public object Valid { get; set; }
        [JsonProperty("site")]
        public string Site { get; set; }
        [JsonProperty("upLimitEnable")]
        public bool UpLimitEnabled { get; set; }
        [JsonProperty("downLimitEnable")]
        public bool DownLimitEnabled { get; set; }
        [JsonProperty("byteQuotaEnable")]
        public bool TrafficLimitEnabled { get; set; }
        /// <summary>
        /// Traffic limit in megabytes
        /// </summary>
        [JsonProperty("byteQuota")]
        public int? TrafficLimit { get; set; }
        /// <summary>
        /// Upload speed limit in kilobit per second
        /// </summary>
        [JsonProperty("upLimitRate")]
        public int? UpLimitRate { get; set; }
        /// <summary>
        /// Download dpeed limit in kilobit per second
        /// </summary>
        [JsonProperty("downLimitRate")]
        public int? DownLimitRate { get; set; }
        [JsonProperty("byteQuotaleft")]
        public bool? TrafficLimitLeft { get; set; }
    }
}
