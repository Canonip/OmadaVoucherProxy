using Newtonsoft.Json;
using System.Collections.Generic;

namespace OmadaAPI.Model.DTO
{
    public class QueryVouchersResult
    {
        [JsonProperty("queryData")]
        public QueryVouchersParams Request { get; set; }
        [JsonProperty("totalRows")]
        public int TotalVouchers { get; set; }
        [JsonProperty("data")]
        public ICollection<Voucher> Vouchers { get; set; }
    }
}
