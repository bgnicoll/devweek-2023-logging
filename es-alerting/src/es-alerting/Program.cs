using Newtonsoft.Json;
using System.Text;

namespace es_alerting
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var webhookUrl = "";
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(webhookUrl)
            };

            var discordMessage = new
            {
                usename = "Brandon Bot 5000",
                content = "Webhook is working"
            };

            var payload = JsonConvert.SerializeObject(discordMessage);
            var bodyContent = new StringContent(payload, Encoding.UTF8, "application/json");

            await httpClient.PostAsync(string.Empty, bodyContent);
        }
    }
}