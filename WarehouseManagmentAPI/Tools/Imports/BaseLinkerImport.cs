using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;

namespace WarehouseManagmentAPI.Tools.Imports
{
    public static class BaseLinkerImport
    {
        private static string _apiEntryPoint = @"https://api.baselinker.com/connector.php";
        private static string _methodName = "getOrders";
        public static List<ResponsePage> OrdersImport()
        {
            var data = new
            {
                order_status_id = Config.status_id
            };

            object parameters = data;

            HttpClient httpClient = new HttpClient();
            Uri requestUri = new Uri(_apiEntryPoint);
            httpClient.DefaultRequestHeaders.Add("X-BLToken", Config.BLtoken);

            string content = string.Join("&", new Dictionary<string, string>()
            {
                {
                    "method",
                    _methodName
                },
                {
                 "developer_id",
                 "99867"
                }
            }.Select(i => i.Key + "=" + i.Value).ToArray());

            if (parameters != null)
                content = content + "&parameters=" + WebUtility.UrlEncode(JsonConvert.SerializeObject(parameters));

            StringContent stringContent = new StringContent(content, null, "application/x-www-form-urlencoded");
            string result = httpClient.PostAsync(requestUri, stringContent).Result.Content.ReadAsStringAsync().Result;

            var r = string.Empty;
            ResponseOrder responsePage;
            try
            {
                responsePage = JsonConvert.DeserializeObject<ResponseOrder>(result);
                if (responsePage != null)
                    return responsePage.orders;
            }
            catch { }

            return null;
        }
        public static ResponsePage OrderImport(int id)
        {
            var data = new
            {
                order_id = id
            };
            object parameters = data;

            HttpClient httpClient = new HttpClient();
            Uri requestUri = new Uri(_apiEntryPoint);
            httpClient.DefaultRequestHeaders.Add("X-BLToken", Config.BLtoken);

            string content = string.Join("&", new Dictionary<string, string>()
            {
                {
                    "method",
                    _methodName
                },
                {
                 "developer_id",
                 "99867"
                }
            }.Select(i => i.Key + "=" + i.Value).ToArray());

            if (parameters != null)
                content = content + "&parameters=" + WebUtility.UrlEncode(JsonConvert.SerializeObject(parameters));

            StringContent stringContent = new StringContent(content, null, "application/x-www-form-urlencoded");
            string result = httpClient.PostAsync(requestUri, stringContent).Result.Content.ReadAsStringAsync().Result;


            ResponseOrder responsePage;
            try
            {
                responsePage = JsonConvert.DeserializeObject<ResponseOrder>(result);
                if (responsePage != null)
                    return responsePage.orders.First();
            }
            catch { }

            return null;
        }
    }

    public class ResponseOrder
    {
        public string status { get; set; }
        public List<ResponsePage> orders { get; set; }
    }
    public class ResponsePage
    {
        public int order_status_id { get; set; }
        public int order_id { get; set; }
        public List<ResponseProduct> products { get; set; }
    }
    public class ResponseProduct
    {
        public string sku { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
    }
}
