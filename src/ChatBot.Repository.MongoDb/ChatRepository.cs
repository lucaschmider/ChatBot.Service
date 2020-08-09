using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatBot.Repository.Contracts;
using ChatBot.Repository.Contracts.Models;
using ChatBot.Repository.MongoDb.Configurations;
using ChatBot.Repository.MongoDb.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Shouldly;

namespace ChatBot.Repository.MongoDb
{
    public class ChatRepository : RepositoryBase<InternalUser>, IChatRepository
    {

        private readonly ILogger<ChatRepository> _logger;

        public ChatRepository(
            ILogger<ChatRepository> logger, 
            MongoDbConfiguration configuration): base(configuration)
        {
            logger.ShouldNotBeNull();
            configuration.ShouldNotBeNull();
            _logger = logger;
        }

        public Task SendMessageAsync(ChatMessage message)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ChatMessage>> GetChatMessagesForUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
