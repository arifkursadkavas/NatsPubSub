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
using snow_app.Services;

namespace snow_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Message>>  Get()
        {
            //_logger.LogDebug("Get request received to present messages");
            return MessageService.GetAllMessagesFromCache();
        }

        [HttpPost]
        public async Task<Message>  Post(Message message)
        {
            //_logger.LogDebug("Post request received", message);
            return  await MessageService.PublishMessageToTopic(message);
        }

    }
}
