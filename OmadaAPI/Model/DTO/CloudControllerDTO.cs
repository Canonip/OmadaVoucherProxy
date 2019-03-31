using Newtonsoft.Json;

namespace OmadaAPI.Model.DTO
{
    public class QueryCloudControllersResult
    {
        public int totalNum { get; set; }
        [JsonProperty("deviceList")]
        public CloudControllerDTO[] OmadaControllers { get; set; }
    }

    public class CloudControllerDTO
    {
        [JsonProperty("deviceId")]
        public string Id { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }
        [JsonProperty("alias")]
        public string FriendlyName { get; set; }
        [JsonProperty("deviceMac")]
        public string MacAddress { get; set; }
        [JsonProperty("deviceModel")]
        public string HardwareModel { get; set; }
        [JsonProperty("deviceHwVer")]
        public string HardvareVersion { get; set; }
        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }
        [JsonProperty("fwVer")]
        public string FirmwareVersion { get; set; }
        [JsonProperty("appServerUrl")]
        public string AppServerUrl { get; set; }
        [JsonProperty("isSameRegion")]
        public bool IsSameRegion { get; set; }
        [JsonProperty("role")]
        public int Role { get; set; }
        [JsonProperty("controllerVersion")]
        public string ControllerVersion { get; set; }
        [JsonProperty("alerts")]
        public int AlertCount { get; set; }
        [JsonProperty("sites")]
        public int SiteCount { get; set; }
        [JsonProperty("devices")]
        public int DeviceCount { get; set; }
        [JsonProperty("clients")]
        public int ClientCount { get; set; }
        [JsonProperty("ip")]
        public string IpAddress { get; set; }
        [JsonProperty("httpPort")]
        public int Port { get; set; }
        [JsonProperty("canUpgrade")]
        public bool CanBeUpgraded { get; set; }
        [JsonProperty("configured")]
        public bool IsConfigured { get; set; }
    }

}
