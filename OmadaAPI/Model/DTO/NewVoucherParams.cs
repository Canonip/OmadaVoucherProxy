using Newtonsoft.Json;
using System;

namespace OmadaAPI.Model.DTO
{
    public class NewVoucherParams : BaseParams
    {
        /// <summary>
        /// Code length of every generated voucher.
        /// Needs to be between 6 and 10
        /// </summary>
        [JsonProperty("codeLength")]
        public int VoucherCodeLength { get; set; } = 6;
        /// <summary>
        /// Amount of vouchers to generate
        /// </summary>
        [JsonProperty("amount")]
        public int Amount { get; set; } = 1;
        [JsonProperty("type")]
        public int Type => 1;
        [JsonProperty("upLimitEnable")]
        public bool UpLimitEnabled { get; set; } = false;
        [JsonProperty("downLimitEnable")]
        public bool DownLimitEnabled { get; set; } = false;
        [JsonProperty("byteQuotaEnable")]
        public bool TrafficLimitEnabled { get; set; } = false;
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
        [JsonProperty("duration"), Obsolete("This is for JsonConvert only, use DurationTime instead")]
        public int DurationMinutes { get { return (int)DurationTime.TotalMinutes; } set { DurationTime = TimeSpan.FromMinutes(value); } }
        [JsonIgnore]
        public TimeSpan DurationTime { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        /// <summary>
        /// Amount of devices every voucher will support
        /// </summary>
        [JsonProperty("quota")]
        public int MaxDevices { get; set; } = 1;

        public NewVoucherParams()
        {
            Method = "addVoucher";
        }
        public QueryVouchersParams ToQueryVouchersParams()
        {
            return new QueryVouchersParams() { CurrentPageSize = Amount };
        }
    }

}
