using ChatBot.Repository.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.Repository.MongoDb
{
    public static class MongoDbModule

    {
        public static IServiceCollection AddMongoDbModule(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
            return services;
        }
    }
}