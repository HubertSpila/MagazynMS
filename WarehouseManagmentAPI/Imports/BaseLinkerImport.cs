using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace WarehouseManagmentAPI.Imports
{
    public static class BaseLinkerImport
    {
        public static void OrdersImport(string id)
        {

            string apiEntryPoint = @"https://api.baselinker.com/connector.php";
            string token = "1004196-1003808-04RZ7WJGYMR5OOHFM43NXYKSPKCHTW7NI1YYJESN8D5MHF21V34RZF1RWS622QW5";
            string methodName = "getOrders";

            var data = new
            {
                order_id = id
            };
            object parameters = (object)data;

            HttpClient httpClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            Uri requestUri = new Uri(apiEntryPoint);
            httpClient.DefaultRequestHeaders.Add("X-BLToken", token);

            string content = string.Join("&", new Dictionary<string, string>()
            {
                {
               "method",
               methodName
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
                    r = responsePage.orders.First().order_page;
            }
            catch
            {
            }
        }
    }

    public class ResponseOrder
    {
        public string status { get; set; }
        public List<ResponsePage> orders { get; set; }

    }
    public class ResponsePage
    {
        public string order_page { get; set; }
    }
}
