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
    public class SubscriptionProvider : IDisposable
    {
        public HubConnection Connection { get; internal set; }

        public IHubProxy HubProxy { get; internal set; }

        public string BaseUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._baseUrl))
                {
                    return AppSetting.BaseUrl;
                }

                return this._baseUrl;
            }
            set => this._baseUrl = value;
        }

        private string _baseUrl;

        private bool _disposed;

        public SubscriptionProvider()
        {
            if (string.IsNullOrWhiteSpace(this.BaseUrl))
            {
                this.BaseUrl = AppSetting.BaseUrl;
            }
        }

        public void CloseConnection()
        {
            this.Connection.Stop();
        }

        public bool CreateEventSubscription(SubscriptionRequest              subscription,
                                            Action<NotifySubscriptionStatus> action)
        {
            this.HubProxy.On("notifySubscriptionStatus",
                             (NotifySubscriptionStatus o) =>
                             {
                                 Console.WriteLine("接收 notifySubscriptionStatus 事件");
                                 action.Invoke(o);
                             });

            var client = new RestClient(this.BaseUrl);
            var url    = $"/api/sr/eventssubscriptions/channelize/{subscription.RequestId}/{subscription.ConnectionId}";

            var request = new RestRequest(url, Method.POST);

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(subscription.AccessToken,
                                                                                     "Bearer");

            //request.AddHeader("Authorization", $"bearer {token}");
            //request.AddHeader("Content-Type", "application/json");

            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }

        public bool CreateValueSubscription(SubscriptionRequest              subscription,
                                            string[]                         objectIds,
                                            Action<IEnumerable<NotifyValue>> action)
        {
            this.HubProxy.On("notifyValues",
                             (IEnumerable<NotifyValue> o) =>
                             {
                                 Console.WriteLine("接收 notifyValues 事件");
                                 action.Invoke(o);
                             });

            var client = new RestClient(this.BaseUrl);
            var url    = $"/api/sr/valuessubscriptions/channelize/{subscription.RequestId}/{subscription.ConnectionId}";

            var request = new RestRequest(url, Method.POST);

            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(subscription.AccessToken,
                                                                                     "Bearer");

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

        public async Task<HubConnection> OpenConnectionAsync()
        {
            this.Connection = new HubConnection($"{this.BaseUrl}/signalr/hubs");
            this.HubProxy   = this.Connection.CreateHubProxy("norisHub");

            await this.Connection.Start().ConfigureAwait(false);
            return this.Connection;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this.Connection.Dispose();
                }

                this._disposed = true;
            }
        }
    }
}