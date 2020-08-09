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
    /// <inheritdoc />
    public class UserRepository : RepositoryBase<InternalUser>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger, MongoDbConfiguration configuration): base(configuration)
        {
            logger.ShouldNotBeNull();
            configuration.ShouldNotBeNull();
            
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<User> GetUserDetailsAsync(string userId)
        {
            _logger.LogInformation($"Loading data for user with id {userId}.");
            var user = await Collection
                .FindAsync(u => u.Uid == userId)
                .ConfigureAwait(false);
            return user
                .FirstOrDefault()
                .Map();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogInformation($"Loading all users data.");
            var user = await Collection
                .FindAsync(u => true)
                .ConfigureAwait(false);
            return user.ToList().Select(u => u.Map());
        }

        /// <inheritdoc />
        public async Task CreateUserDetailsAsync(User details)
        {
            _logger.LogInformation($"Writing details for user {details.Uid}");
            await Collection
                .InsertOneAsync(details.Map())
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteUserDetailsAsync(string userId)
        {
            _logger.LogInformation($"Removing data of user {userId}");
            await Collection
                .DeleteOneAsync(user => user.Uid == userId)
                .ConfigureAwait(false);
        }
    }
}