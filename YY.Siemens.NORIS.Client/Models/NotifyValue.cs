using System;
using Newtonsoft.Json;

namespace YY.Siemens.NORIS.Client.Models
{
    public class NotifyValue
    {
        [JsonProperty("DataType")]
        public string DataType { get; set; }

        [JsonProperty("Value")]
        public NotifyContent Value { get; set; }

        [JsonProperty("ErrorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("IsArray")]
        public bool IsArray { get; set; }

        [JsonProperty("SubscriptionKey")]
        public int SubscriptionKey { get; set; }

        public class NotifyContent
        {
            [JsonProperty("Value")]
            public string Value { get; set; }

            [JsonProperty("Quality")]
            public string Quality { get; set; }

            [JsonProperty("QualityGood")]
            public bool QualityGood { get; set; }

            [JsonProperty("Timestamp")]
            public DateTime Timestamp { get; set; }
        }
    }
}