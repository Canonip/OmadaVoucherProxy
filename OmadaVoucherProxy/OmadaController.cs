using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OmadaVoucherProxy
{
    public class OmadaController
    {
        CookieContainer cookies;
        readonly HttpClientHandler handler;
        HttpClient client;
        public OmadaController(Uri omadaHost)
        {
            OmadaHost = omadaHost;
            cookies = new CookieContainer();
            handler = new HttpClientHandler
            {
                CookieContainer = cookies
            };
            client = new HttpClient(handler) { BaseAddress = OmadaHost };
        }
        public Uri OmadaHost { get; set; }
        const string HOTSPOT_ENDPOINT = "/web/v1/hotspot";
        const string CREATE_VOUCHER_ENDPOINT = "/web/v1/hotspot?createVoucherModel";
        const string QUERY_VOUCHER_ENDPOINT = "/web/v1/hotspot?voucherManagerStore";
        const string LOGIN_ENDPOINT = "/api/user/login?ajax";
        public async Task LoginAsync(LoginParams login)
        {
            //Get Session ID
            (await client.GetAsync("/")).EnsureSuccessStatusCode();
            //Login
            var response = await client.PostAsync(LOGIN_ENDPOINT, new StringContent(login.ToJson()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var responseText = await response.Content.ReadAsStringAsync();
                    var json = JsonConvert.DeserializeObject<Response<LoginResult>>(responseText);
                    json.EnsureSuccessStatusCode();
                    //Save token to cookies
                    cookies.Add(new Cookie("token", json.Result.Token, "/", OmadaHost.Host));
                }
                catch (JsonException e)
                {
                    throw new Exception("Error Logging in, Returned Data is no Json");
                }

            }
            else
            {
                throw new Exception("Error Logging in, Returned " + response.StatusCode);
            }
        }

        public async Task<ICollection<Voucher>> GenerateVouchersAsync(NewVoucherParams settings)
        {
            var token = cookies.GetCookies(OmadaHost).Cast<Cookie>().SingleOrDefault(x => x.Name.Equals("token"));
            if (token == null)
                throw new Exception("No token present");

            //Create New Vouchers
            {
                var response = await client.PostAsync($"{CREATE_VOUCHER_ENDPOINT}&token={token.Value}", new StringContent(settings.ToJson()));
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response<object>>(json);
                obj.EnsureSuccessStatusCode();
            }
            var queryVouchersParams = new QueryVouchersParams()
            {
                SortName = SortName.createdTime,
                SortOrder = SortOrder.desc,
                CurrentPageSize = settings.Amount
            };

            {
                var a = queryVouchersParams.ToJson();
                var response = await client.PostAsync($"{QUERY_VOUCHER_ENDPOINT}&token={token.Value}", new StringContent(queryVouchersParams.ToJson()));
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<Response<QueryVouchersResult>>(json);
                obj.EnsureSuccessStatusCode();
                return obj.Result.Vouchers;
            }
        }
    }
}