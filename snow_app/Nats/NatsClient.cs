using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;

using NATS.Client;

namespace snow_app.Nats
{
    
    public class NatsClient<T>
    {
        private IEncodedConnection _connection;

        public NatsClient(Options options)
        {   
            _connection =  CreateNatsConnection(options);
        }

        public NatsClient()
        {
            _connection = CreateNatsConnection(null);
        }

        public T  Publish(String topic, T  msg)
        {
            _connection.Publish(topic, msg);
            return msg;
        }

        public void Subscribe(String topic, EventHandler<EncodedMessageEventArgs> handler)
        {
            IAsyncSubscription s = _connection.SubscribeAsync(topic, handler);
        }

        private IEncodedConnection CreateNatsConnection(Options options)
        {
            ConnectionFactory cf = new ConnectionFactory();

            IEncodedConnection conn = options != null ? cf.CreateEncodedConnection(options) : cf.CreateEncodedConnection();
      
            conn.OnDeserialize = JsonDeserializer;
            conn.OnSerialize = JsonSerializer;

            return conn;
        }

        private byte[] JsonSerializer(object obj) // Serializer and Deserializer needed due to restriction of dotnet core version
        {
            if (obj == null)
                return null;

            var serializer = new DataContractJsonSerializer(typeof(Message));

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return stream.ToArray();
            }
        }

        private  object JsonDeserializer(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(Message));
                stream.Write(buffer, 0, buffer.Length);
                stream.Position = 0;
                return serializer.ReadObject(stream);
            }
        }
    }
}
