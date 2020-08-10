using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ChatRepository : RepositoryBase<InternalMessage>, IChatRepository
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

        public async Task SendMessageAsync(ChatMessage message)
        {
            _logger.LogInformation("Creating new message");
            await Collection.InsertOneAsync(message.Map());
        }

        public async Task<IEnumerable<ChatMessage>> GetChatMessagesForUserAndDeleteAsync(string userId)
        {
            var now = DateTime.Now;
            _logger.LogInformation($"Getting messages new messages for user {userId}");
            var messages = await Collection
                .FindAsync(message => message.CreateDate <= now && message.Recipient.Equals(userId))
                .ConfigureAwait(false);
            await Collection.DeleteManyAsync(message => message.CreateDate <= now);
            return messages.ToList().Select(message => message.Map());
        }
    }
}
