using ChatBot.MessageInterpreter.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.MessageInterpreter.DialogFlow
{
    public static class DialogFlowModule
    {
        public static IServiceCollection AddDialogFlowModule(this IServiceCollection services,
            DialogFlowConfiguration configuration)
        {
            services
                .AddSingleton(configuration)
                .AddSingleton<IMessageInterpreter, MessageInterpreter>();
            return services;
        }
    }
}