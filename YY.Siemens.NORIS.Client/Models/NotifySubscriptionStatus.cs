using Newtonsoft.Json;

namespace YY.Siemens.NORIS.Client.Models
{
    public class NotifySubscriptionStatus
    {
        [JsonProperty("Key")]
        public int Key { get; set; }

        [JsonProperty("PropertyId")]
        public string PropertyId { get; set; }

        [JsonProperty("ErrorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("objectOrPropertyId")]
        public string ObjectOrPropertyId { get; set; }

        [JsonProperty("RequestId")]
        public string RequestId { get; set; }

        public class Link
        {
            [JsonProperty("_links")]
            public Link[] Links { get; set; }

            [JsonProperty("Rel")]
            public string Rel { get; set; }

            [JsonProperty("Href")]
            public string Href { get; set; }

            [JsonProperty("IsTemplated")]
            public bool IsTemplated { get; set; }
        }
    }
}