using Newtonsoft.Json;
using OmadaAPI.Exceptions;
using OmadaAPI.Helpers;
using OmadaAPI.Model;
using OmadaAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OmadaAPI.Controllers
{
    public class CloudConnector
    {
        private readonly CookieContainer cookies;
        private readonly HttpMessageHandler handler;
        private readonly HttpClient client;
        private readonly Uri TpLinkUri = new Uri("https://omada.tplinkcloud.com");
        private string LoginEndpoint => "/api/user/login?form=login";
        private string DeviceEndpoint => $"/device?token={token}";
        private string GenerateVoucherEndpoint => $"/web/v1/hotspot?createVoucherModel&token={token}&serverKey=";
        private string QueryVoucherEndpoint => $"/web/v1/hotspot?voucherManagerStore&token={token}&serverKey=";
        private string ConnectToDeviceEndpoint => $"/startLaunch?token={token}";
        private string LaunchStatusEndpoint => $"/web/v1/getLaunchStatus?ajax&token={token}";
        private string token;
        private readonly string username;
        private readonly string password;
        /// <summary>
        /// Sets the default controller, which will be used for queries where no cloud controller is specified
        /// </summary>
        public string DefaultCloudControllerId { get; set; }
        private CloudConnector()
        {
            cookies = new CookieContainer();
            handler = new LoggingHandler(new HttpClientHandler
            {
                CookieContainer = cookies,
                AllowAutoRedirect = false
            });
            client = new HttpClient(handler)
            {
                BaseAddress = TpLinkUri
            };
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        public CloudConnector(CloudLogin credentials) : this()
        {
            username = credentials.Username;
            password = credentials.Password;
        }

        public async Task LoginAsync()
        {
            //Login
            HttpResponseMessage response;
            var loginParams = new Dictionary<string, string>()
            {
                { "username", username },
                { "password", password },
                { "rememberMe", string.Empty }
            };
            response = await client.PostAsync(LoginEndpoint, new FormUrlEncodedContent(loginParams));
            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                if (!responseText.StartsWith("{"))
                {
                    //not json -> error
                }
                var json = JsonConvert.DeserializeObject<BaseResponse<LoginResult>>(responseText);
                json.EnsureSuccessStatusCode();
                token = json.Result.Token;
            }
            else
            {
                throw new OmadaResponseException("Error Logging in, Statuscode: " + response.StatusCode);
            }
        }
        public async Task<ICollection<CloudControllerDTO>> GetCloudControllersAsync()
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "page", "0" },
                { "limit", "20" },
                { "operation", "loadByPage" },
                { "deviceType", "0" }
            };
            var response = await client.PostAsync(DeviceEndpoint, new FormUrlEncodedContent(queryParams));
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<BaseResponse<QueryCloudControllersResult>>(responseText);
            json.EnsureSuccessStatusCode();
            return json.Result.OmadaControllers;
        }
        public async Task<ICollection<Voucher>> GenerateVouchersAsync(NewVoucherParams voucherParams)
        {
            if (string.IsNullOrEmpty(DefaultCloudControllerId))
            {
                throw new ArgumentException("DefaultCloudControllerId is null");
            }
            return await GenerateVouchersAsync(voucherParams, DefaultCloudControllerId);
        }
        public async Task ConnectToCloudControllerAsync(string cloudControllerId = null)
        {
            cloudControllerId = CheckCloudControllerId(cloudControllerId);
            //If already Ready, no need to connect again
            if (await CheckIfConnectedAsync(cloudControllerId))
                return;
            var queryParams = new Dictionary<string, string>()
            {
                {"deviceId", cloudControllerId }
            };
            var response = await client.PostAsync(ConnectToDeviceEndpoint, new FormUrlEncodedContent(queryParams));
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<BaseResponse>(responseText);
            json.EnsureSuccessStatusCode();
            //bool spinUntil = System.Threading.SpinWait.SpinUntil(() => CheckIfConnectedAsync(cloudControllerId), TimeSpan.FromSeconds(5));

            var status = await RetryUntilSuccessOrTimeout(async () => await CheckIfConnectedAsync(cloudControllerId), TimeSpan.FromSeconds(10));
            if (!status)
            {
                throw new Exception("Could not connect to Controller, Timeout reached");
            }
        }
        public async Task<ICollection<Voucher>> GenerateVouchersAsync(NewVoucherParams voucherParams, string cloudControllerId = null)
        {
            cloudControllerId = CheckCloudControllerId(cloudControllerId);
            //Generate Vouchers
            {
                var response = await client.PostAsync(GenerateVoucherEndpoint + cloudControllerId, new StringContent(voucherParams.ToJson()));
                response.EnsureSuccessStatusCode();
                var responseText = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<BaseResponse>(responseText);
                json.EnsureSuccessStatusCode();
            }
            //Query Vouchers
            {
                var response = await client.PostAsync(QueryVoucherEndpoint + cloudControllerId, new StringContent(voucherParams.ToQueryVouchersParams().ToJson()));
                response.EnsureSuccessStatusCode();
                var responseText = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<BaseResponse<QueryVouchersResult>>(responseText);
                json.EnsureSuccessStatusCode();
                return json.Result.Vouchers;
            }
        }
        private async Task<bool> CheckIfConnectedAsync(string cloudControllerId = null)
        {
            cloudControllerId = CheckCloudControllerId(cloudControllerId);
            var response = await client.PostAsync(LaunchStatusEndpoint, new StringContent(new LaunchStatusParams() { CloudControllerId = cloudControllerId}.ToJson()));
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<BaseResponse<LaunchStatusResult>>(responseText);
            json.EnsureSuccessStatusCode();
            return json.Result.Status == LaunchStatus.Ready;
        }
        private string CheckCloudControllerId(string cloudControllerId)
        {
            if (cloudControllerId == null)
            {
                if (string.IsNullOrEmpty(DefaultCloudControllerId))
                {
                    throw new ArgumentNullException("cloudControllerId and DefaultCloudControllerId are null");
                }
                return DefaultCloudControllerId;
            }
            return cloudControllerId;
        }
        private async Task<bool> RetryUntilSuccessOrTimeout(Func<Task<bool>> task, TimeSpan timeSpan)
        {
            bool success = false;
            int elapsed = 0;
            while ((!success) && (elapsed < timeSpan.TotalMilliseconds))
            {
                Thread.Sleep(1000);
                elapsed += 1000;
                success = await task();
            }
            return success;
        }
    }
}
