using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snow_app.Services
{
    public interface IMessageService
    {
        List<Message> GetAllMessagesFromCache();
        Task<Message> PublishMessageToTopic(Message msg);
    }
}
