using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace OmadaVoucherProxy
{
    public class BaseParams
    {
        [JsonIgnore]
        public string Method { get; protected set; }
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
    public class RequestPayload
    {
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("params")]
        public object Params { get; set; }

    }

    public class NewVoucherParams : BaseParams
    {
        [JsonProperty("codeLength")]
        public int CodeLength { get; set; } = 6;
        [JsonProperty("amount")]
        public int Amount { get; set; } = 1;
        [JsonProperty("type")]
        public int Type { get; set; } = 1;
        [JsonProperty("upLimitEnable")]
        public bool UpLimitEnable { get; set; } = false;
        [JsonProperty("downLimitEnable")]
        public bool DownLimitEnable { get; set; } = false;
        [JsonProperty("byteQuotaEnable")]
        public bool ByteQuotaEnable { get; set; } = false;
        [JsonProperty("duration"), Obsolete("This is for Json only, use DurationTime instead")]
        public int DurationMinutes { get { return (int)DurationTime.TotalMinutes; } set { DurationTime = TimeSpan.FromMinutes(value); } }
        [JsonIgnore]
        public TimeSpan DurationTime { get; set; }
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("quota")]
        public int Quota { get; set; } = 1;

        public NewVoucherParams()
        {
            Method = "addVoucher";
        }
    }
    public class LoginParams : BaseParams
    {
        [JsonProperty("name")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        public LoginParams()
        {
            Method = "login";
        }
        public static LoginParams FromCredentials(string user, string password)
        {
            return new LoginParams()
            {
                UserName = user,
                Password = password
            };
        }
    }


    public class Response<T>
    {
        [JsonIgnore]
        public bool IsSuccessStatusCode => StatusCode == 0;
        public void EnsureSuccessStatusCode()
        {
            if (!IsSuccessStatusCode)
                throw new Exception("Response is no Success! Error: " + Message);
        }
        [JsonProperty("errorCode")]
        public int StatusCode { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("result")]
        public T Result { get; set; }
    }

    public class LoginResult
    {
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }



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
    public enum SortOrder
    {
        asc, desc
    }
    public enum SortName
    {
        createdTime, codeAlias, note, duration
    }

    public class QueryVouchersResult
    {
        [JsonProperty("queryData")]
        public QueryVouchersParams Request { get; set; }
        [JsonProperty("totalRows")]
        public int TotalVouchers { get; set; }
        [JsonProperty("data")]
        public Voucher[] Vouchers { get; set; }
    }

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
        public long StartTime { get; set; }
        [JsonProperty("endTime")]
        public long EndTime { get; set; }
        [JsonProperty("valid")]
        public object Valid { get; set; }
        [JsonProperty("site")]
        public string Site { get; set; }
        [JsonProperty("upLimitEnable")]
        public bool UpLimitEnable { get; set; }
        [JsonProperty("downLimitEnable")]
        public bool DownLimitEnable { get; set; }
        [JsonProperty("byteQuotaEnable")]
        public bool ByteQuotaEnable { get; set; }
        [JsonProperty("upLimit")]
        public double UpLimit { get; set; }
        [JsonProperty("downLimit")]
        public double DownLimit { get; set; }
        [JsonProperty("ByteQuota")]
        public double ByteQuota { get; set; }
        [JsonProperty("byteQuotaleft")]
        public bool ByteQuotaleft { get; set; }
    }


}
