using System.Linq;
using ChatBot.Repository.MongoDb.Configurations;
using MongoDB.Driver;
using Shouldly;

namespace ChatBot.Repository.MongoDb
{
    public abstract class RepositoryBase<T>
    {
        internal readonly IMongoCollection<T> Collection;

        protected RepositoryBase(MongoDbConfiguration configuration)
        {
            var client = new MongoClient(configuration.ConnectionString);
            var database = client.GetDatabase(configuration.Database);
            var collectionNameAttribute = (CollectionNameAttribute) typeof(T)
                .GetCustomAttributes(typeof(CollectionNameAttribute), false)
                .FirstOrDefault();

            collectionNameAttribute.ShouldNotBeNull();

            Collection = database.GetCollection<T>(collectionNameAttribute.CollectionName);
        }
    }
}