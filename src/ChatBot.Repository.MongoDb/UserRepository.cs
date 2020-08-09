﻿using System.Collections.Generic;
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
    public class UserRepository : IUserRepository
    {
        private const string CollectionName = "users";
        private readonly IMongoCollection<InternalUser> _collection;

        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger, MongoDbConfiguration configuration)
        {
            logger.ShouldNotBeNull();
            configuration.ShouldNotBeNull();

            var client = new MongoClient(configuration.ConnectionString);
            var database = client.GetDatabase(configuration.Database);

            _logger = logger;
            _collection = database.GetCollection<InternalUser>(CollectionName);
        }

        /// <inheritdoc />
        public async Task<User> GetUserDetailsAsync(string userId)
        {
            _logger.LogInformation($"Loading data for user with id {userId}.");
            var user = await _collection
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
            var user = await _collection
                .FindAsync(u => true)
                .ConfigureAwait(false);
            return user.ToList().Select(u => u.Map());
        }

        /// <inheritdoc />
        public async Task CreateUserDetailsAsync(User details)
        {
            _logger.LogInformation($"Writing details for user {details.Uid}");
            await _collection
                .InsertOneAsync(details.Map())
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteUserDetailsAsync(string userId)
        {
            _logger.LogInformation($"Removing data of user {userId}");
            await _collection
                .DeleteOneAsync(user => user.Uid == userId)
                .ConfigureAwait(false);
        }
    }
}