using System;
using System.Runtime.Serialization;

namespace snow_app
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string TextMessage { get; set; }
        [DataMember]
        public DateTime TimeStamp { get; set; }
    }
}
