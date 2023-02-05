using Newtonsoft.Json;

namespace es_alerting
{
    public class LogExample
    {
        [JsonProperty("timestamp")]
        public DateTime timestamp { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
        [JsonProperty("raiseTheAlarm")]
        public bool raiseTheAlarm { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("customerFavoriteFood")]
        public string FavoriteFood { get; set; }
    }
}
