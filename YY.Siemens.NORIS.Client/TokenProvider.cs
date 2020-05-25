using System.Net;
using Newtonsoft.Json;
using RestSharp;
using YY.Siemens.NORIS.Client.Models;

namespace YY.Siemens.NORIS.Client
{
    public class TokenProvider
    {
        public string BaseUrl { get; set; }

        public Token Login(string id, string password)
        {
            Token result         = null;
            var   url            = "/api/token";
            var   client         = new RestClient(this.BaseUrl);
            var   request        = new RestRequest(url, Method.POST);
            var   requestContent = $"grant_type=password&username={id}&password={password}";

            request.AddHeader("Content-Type", "text/plain");
            request.AddParameter("text/plain", requestContent, ParameterType.RequestBody);
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return result;
            }

            var responseContent = response.Content;
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return result;
            }

            result = JsonConvert.DeserializeObject<Token>(responseContent);
            return result;
        }
    }
}