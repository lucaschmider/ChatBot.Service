using ChatBot.Repository.Contracts;
using ChatBot.Repository.MongoDb.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.Repository.MongoDb
{
    public static class MongoDbModule

    {
        public static IServiceCollection AddMongoDbModule(this IServiceCollection services,
            MongoDbConfiguration configuration)
        {
            services
                .AddSingleton(configuration)
                .AddSingleton<IKnowledgeRepository, KnowledgeRepository>()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IChatRepository, ChatRepository>();
            return services;
        }
    }
}