using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using YY.Siemens.NORIS.Client.Models;

namespace YY.Siemens.NORIS.Client.Demp.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //匯入設定
            AppSetting.Import(@"d:\app.json");

            ////手動設定
            //AppSetting.BaseUrl  = "http://aa.bb";
            //AppSetting.Id       = "your id";
            //AppSetting.Password = "your password";

            var tokenProvider        = new TokenProvider();
            var subscriptionProvider = new SubscriptionProvider();
            var token                = tokenProvider.Login(AppSetting.Id, AppSetting.Password);
            var connection           = subscriptionProvider.OpenConnectionAsync().Result;
            var connectionId         = connection.ConnectionId;
            if (connection.State == ConnectionState.Connected)
            {
                Console.WriteLine($"已連線, Id={connectionId}");
            }

            var subscriptionRequest = new SubscriptionRequest
            {
                AccessToken  = token.AccessToken,
                ConnectionId = connectionId,
                RequestId    = "9565ca41-8556-4dbb-94cc-1b89451a5db5"
            };
            var objectIds = new[] {"System1:GmsDevice_1_2098227_8388656"};

            subscriptionProvider.CreateValueSubscription(subscriptionRequest,
                                                         objectIds,
                                                         NotifyValue);
            subscriptionProvider.CreateEventSubscription(subscriptionRequest,
                                                         NotifySubscriptionStatus);

            Console.WriteLine("按任意鍵離開");
            Console.ReadKey();
        }

        private static void NotifySubscriptionStatus(NotifySubscriptionStatus e)
        {
            var json = JsonConvert.SerializeObject(e);
            Console.WriteLine(json);
        }

        private static void NotifyValue(IEnumerable<NotifyValue> e)
        {
            var json = JsonConvert.SerializeObject(e);
            Console.WriteLine(json);
        }
    }
}