using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestAntiParsing
{
    public class Program
    {
        static void Main(string[] args)
        {
            var settings = GetValueFromFail();
            Console.WriteLine($"Url = { settings.Url}");
            Console.WriteLine($"UserAgent = { settings.UserAgent}");
            Console.WriteLine($"Referer = { settings.Referer}");
            Console.WriteLine($"Count = { settings.Count}");
            

            for(int i = 0; i <= settings.Count; i++)
            {
                Console.WriteLine("Send request");
                var result = SendHttp(settings.Url, settings.UserAgent, settings.Referer);
                Console.WriteLine($"Response Status Code =  { result.Result.StatusCode}");
                Console.WriteLine($"Is SuccessStatus Code =  { result.Result.IsSuccessStatusCode}");
                Console.WriteLine($"Content =  { result.Result.Content.ReadAsStringAsync().Result}");
            }

        }


        private static ConfigModel GetValueFromFail(string fileName = "appsettings.json")
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<ConfigModel>(json);
            }
        }

        private static async Task<HttpResponseMessage> SendHttp(string url, string userAgent = null, string referer = null)
        {

            using (var client = new HttpClient())
            {
                if(!string.IsNullOrEmpty(referer))
                    client.DefaultRequestHeaders.Add("Referer", referer);
                if (!string.IsNullOrEmpty(userAgent))
                    client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                var uri = new Uri(url);
                return await client.GetAsync(uri);
            }
        }
    }

    public class ConfigModel
    {
        public string UserAgent { get; set; }
        public string Referer { get; set; }

        public int Count { get; set; } = 100;
        public string Url { get; set; }
    }
}
