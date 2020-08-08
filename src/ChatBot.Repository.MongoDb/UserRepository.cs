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
    /// <inheritdoc />
    public class UserRepository : IUserRepository
    {
        private const string CollectionName = "users";
        private readonly IMongoCollection<UserDetails> _collection;

        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger, MongoDbConfiguration configuration)
        {
            logger.ShouldNotBeNull();
            configuration.ShouldNotBeNull();

            var client = new MongoClient(configuration.ConnectionString);
            var database = client.GetDatabase(configuration.Database);

            _logger = logger;
            _collection = database.GetCollection<UserDetails>(CollectionName);
        }

        /// <inheritdoc />
        public async Task<IUserDetails> GetUserDetailsAsync(string userId)
        {
            _logger.LogInformation($"Loading data for user with id {userId}.");
            var user = await _collection
                .FindAsync(u => u.Uid == userId)
                .ConfigureAwait(false);
            return user.FirstOrDefault();
        }
    }
}