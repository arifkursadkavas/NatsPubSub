using snow_app.Nats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace snow_app.Services
{
    public class MessageService: IMessageService
    {
        private const string CacheKey = "uncheckedMessages";
        private const string TopicName = "topicName";

        public MessageService()
        {

        }

        public  List<Message> GetAllMessagesFromCache()
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);

            List<Message> messages = (List<Message>)cache.Get(CacheKey);

            cache.Remove(CacheKey);

            return messages;
        }

        public  async Task<Message> PublishMessageToTopic(Message msg)
        {
            NatsClient<Message> client = new NatsClient<Message>();

            await Task.Run(() => client.Publish(TopicName, msg));

            return msg;
        }
    }
}
