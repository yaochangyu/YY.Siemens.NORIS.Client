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
            //為了避免敏感性資料上傳到github，所以應該將參數依賴於外部檔案
            //匯入設定
            var appSetting = AppSetting.Import(@"d:\app.json");

            //手動設定，設定參數後，匯出成 josn 檔
            //var appSetting = new AppSetting();

            //appSetting.BaseUrl  = "your url, http or https";
            //appSetting.Id       = "your id";
            //appSetting.Password = "your password";
            //AppSetting.Export(appSetting, @"d:\app.json");

            var tokenProvider = new TokenProvider
            {
                BaseUrl = appSetting.BaseUrl
            };
            var subscriptionProvider = new SubscriptionProvider
            {
                BaseUrl = appSetting.BaseUrl
            };
            var token        = tokenProvider.Login(appSetting.Id, appSetting.Password);
            var connection   = subscriptionProvider.OpenConnectionAsync().Result;
            var connectionId = connection.ConnectionId;
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
            subscriptionProvider.CloseConnection();
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