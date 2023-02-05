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
            var logScanMinutes = 1;

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
                                                                    .GreaterThanOrEquals(DateTime.UtcNow.AddMinutes(-logScanMinutes))),
                                                                f => f
                                                                .Term(x => x.raiseTheAlarm, true)
                                                    )))).Documents.ToList();

            if (searchResults == null || !searchResults.Any())
            {
                return;
            }

            var webhookUrl = "https://discord.com/api/webhooks/1071825350750912512/HB2ciJX71eFbauoB1aGyBGuasHgAqVy90UnyXc8ZUMwC9odKFQHMqnMjUTEQtYCNR1KT";
            //Yes, I know putting this in version control is a risk to the Discord server I created, but I'm willing to take it.
            //It's for learning purposes!

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(webhookUrl)
            };

            var scanTimerPluralityBecauseImPedanticLikeThat = logScanMinutes == 1 ? string.Empty : "s";
            var eventCountPluralityBecauseImPedanticLikeThat = searchResults.Count == 1 ? string.Empty : "s";

            var mainMessage = $"Customer event alarm fired. {searchResults.Count} event{eventCountPluralityBecauseImPedanticLikeThat} in the past {logScanMinutes} minute{scanTimerPluralityBecauseImPedanticLikeThat}:";

            var customerEventData = new List<DiscordWebhookNameValuePair>();
            foreach (var customerEvent in searchResults)
            {
                customerEventData.Add(
                    new DiscordWebhookNameValuePair
                    {
                        Name = $"Name: {customerEvent.CustomerName} ",
                        Value = $"Favorite food: {customerEvent.FavoriteFood}"
                    });
            }

            var discordMessage = new
            {
                content = mainMessage,
                embeds = new[]
                {
                    new { fields = customerEventData }
                }
            };

            var payload = JsonConvert.SerializeObject(discordMessage);
            var bodyContent = new StringContent(payload, Encoding.UTF8, "application/json");

            await httpClient.PostAsync(string.Empty, bodyContent);
        }
    }
}