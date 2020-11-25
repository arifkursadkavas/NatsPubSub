using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NATS.Client;
using snow_app.Nats;

namespace snow_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private const string CacheKey = "uncheckedMessages";

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MessageController> _logger;


        

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Message>>  Get()
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);

            List<Message> messages = (List<Message>)cache.Get(CacheKey);

            cache.Remove(CacheKey);

            return messages;
        }

        [HttpPost]
        public async Task<Message>  Post(Message message)
        {
            NatsClient<Message> client = new NatsClient<Message>();
            
            await Task.Run (() => client.Publish("topicName", message));

            return message;
        }

    }
}
