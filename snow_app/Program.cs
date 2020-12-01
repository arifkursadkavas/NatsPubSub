using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NATS.Client;
using snow_app.Nats;

namespace snow_app
{
    public class Program
    {
        private static IEncodedConnection _connection;
        private const string CacheKey = "uncheckedMessages";
        private const string TopicName = "topicName";

        public static void Main(string[] args)
        {
            SubscribeToNatsTopic();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });



        public static void SubscribeToNatsTopic()
        {

            NatsClient<Message> client = new NatsClient<Message>();

            EventHandler<EncodedMessageEventArgs> handler = (sender, args) =>
            {
                Message obj = (Message)args.ReceivedObject;
                AddToCache(obj);

                System.Console.WriteLine("From: " + obj.UserName);
                System.Console.WriteLine("Message: " + obj.TextMessage);
                System.Console.WriteLine("Time: " + obj.TimeStamp);
            };

            client.Subscribe(TopicName, handler);

        }

        private static void AddToCache(Message obj)
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);

            List<Message> messages = (List<Message>)cache.Get(CacheKey);

            if (messages == null)//Initialize
                messages = new List<Message>();

            messages.Add(obj);
            cache.Add(CacheKey, messages, cacheItemPolicy);
        }
    }
}
