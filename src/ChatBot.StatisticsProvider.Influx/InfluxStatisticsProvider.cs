using ChatBot.StatisticsProvider.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.StatisticsProvider.Influx
{
    public static class InfluxStatisticsProvider
    {
        public static IServiceCollection AddInfluxStatisticsModule(this IServiceCollection services,
            InfluxDbConfiguration configuration)
        {
            return services
                .AddSingleton(configuration)
                .AddSingleton<IStatisticsProvider, StatisticsProvider>();
        }
    }
}