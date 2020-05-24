using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using YY.Siemens.NORIS.Client.Models;

namespace YY.Siemens.NORIS.Client
{
    public class SubscriptionProvider
    {
        public string BaseUrl { get; set; }

        public SubscriptionProvider()
        {
            if (string.IsNullOrWhiteSpace(this.BaseUrl))
            {
                this.BaseUrl = AppSetting.BaseUrl;
            }
        }
        public EventHandler<IEnumerable<NotifyValue>> ValueChanged { get; set; }

        public EventHandler<NotifySubscriptionStatus> SubscriptionStatusChanged { get; set; }

        public bool CreateEventSubscription(string token, Guid requestId, string connectId)
        {
            var client = new RestClient(this.BaseUrl);
            var url    = $"/api/sr/eventssubscriptions/channelize/{requestId}/{connectId}";

            var request = new RestRequest(url, Method.POST);

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");

            //request.AddHeader("Authorization", $"bearer {token}");

            //request.AddHeader("Content-Type", "application/json");
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }

        public async Task<HubConnection> CreateConnection()
        {
            var connection = new HubConnection($"{this.BaseUrl}/signalr/hubs");
            var hubProxy   = connection.CreateHubProxy("norisHub");

            hubProxy.On("notifyValues",
                        (IEnumerable<NotifyValue> o) =>
                        {
                            Console.WriteLine(o);
                            if (this.ValueChanged != null)
                            {
                                this.ValueChanged.Invoke(this, o);
                            }
                        });

            hubProxy.On("notifySubscriptionStatus",
                        (NotifySubscriptionStatus o) =>
                        {
                            Console.WriteLine(o);
                            if (this.SubscriptionStatusChanged != null)
                            {
                                this.SubscriptionStatusChanged.Invoke(this, o);
                            }
                        });

            await connection.Start().ConfigureAwait(false);
            return connection;
        }

        public bool CreateValueSubscription(string token, Guid requestId, string connectId, params string[] objectIds)
        {
            var client = new RestClient(this.BaseUrl);
            var url    = $"/api/sr/valuessubscriptions/channelize/{requestId}/{connectId}";

            var request = new RestRequest(url, Method.POST);

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");

            //request.AddHeader("Authorization", $"bearer {token}");

            //request.AddHeader("Content-Type", "application/json");
            var requestContent = JsonConvert.SerializeObject(objectIds);
            request.AddParameter("application/json", requestContent, ParameterType.RequestBody);
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }
    }
}