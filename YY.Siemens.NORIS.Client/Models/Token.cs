using Newtonsoft.Json;

namespace YY.Siemens.NORIS.Client.Models
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string Type { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("user_descriptor")]
        public string UserDescriptor { get; set; }

        [JsonProperty("user_profile")]
        public string UserProfile { get; set; }
    }
}