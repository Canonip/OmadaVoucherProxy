using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmadaAPI.Model.DTO
{
    public class QueryVouchersParams : BaseParams
    {
        public QueryVouchersParams()
        {
            Method = "getGridVouchers";
            CurrentPage = 1;
        }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("sortOrder")]
        public SortOrder? SortOrder { get; set; }
        [JsonProperty("currentPage")]
        public int? CurrentPage { get; set; }
        [JsonProperty("currentPageSize")]
        public int? CurrentPageSize { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("sortName")]
        public SortName? SortName { get; set; }
        [JsonProperty("searchKey")]
        public string SearchKey { get; set; }
    }
}
