using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;
using System.Text;

namespace es_alerting
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var elasticsearchHost = Environment.GetEnvironmentVariable("ELASTICSEARCH_HOSTS") ?? "https://localhost:9200";
            var elasticsearchUsername = Environment.GetEnvironmentVariable("ELASTICSEARCH_USERNAME") ?? "elastic";
            var elasticsearchPassword = Environment.GetEnvironmentVariable("ELASTICSEARCH_PASSWORD") ?? "elastic";
            var defaultElasticsearchIndex = "log-generator-5000";

            var node = new Uri(elasticsearchHost);
            var pool = new SingleNodeConnectionPool(node);
            var settings = new ConnectionSettings(pool)
                .DefaultIndex(defaultElasticsearchIndex)
                .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                .BasicAuthentication(elasticsearchUsername, elasticsearchPassword);
            var elasticClient = new ElasticClient(settings);


            var searchResults = elasticClient.Search<LogExample>(s => s
                                                .Query(q => q
                                                    .Bool(b => b
                                                        .Filter(f => f
                                                                .DateRange(r => r
                                                                    .Field(f => f.timestamp)
                                                                    .GreaterThanOrEquals(DateTime.UtcNow.AddMinutes(-15))),
                                                                f => f
                                                                .Term(x => x.raiseTheAlarm, true)
                                                    )))).Documents.ToList();



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