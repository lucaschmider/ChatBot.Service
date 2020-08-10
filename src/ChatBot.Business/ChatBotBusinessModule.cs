using ChatBot.Business.Contracts.Chat;
using ChatBot.Business.Contracts.Statistics;
using ChatBot.Business.Contracts.User;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.Business
{
    public static class ChatBotBusinessModule
    {
        public static IServiceCollection AddChatBotBusinessModule(this IServiceCollection services)
        {
            services
                .AddTransient<IUserBusiness, UserBusiness>()
                .AddTransient<IStatisticsBusiness, StatisticsBusiness>()
                .AddTransient<IChatBusiness, ChatBusiness>();
            return services;
        }
    }
}