using System.Collections.Generic;
using ChatBot.StatisticsProvider.Contract;
using InfluxDB.Collector;
using Shouldly;

namespace ChatBot.StatisticsProvider.Influx
{
    public class StatisticsProvider : IStatisticsProvider
    {
        private readonly MetricsCollector _collector;
        public StatisticsProvider(InfluxDbConfiguration configuration)
        {
            configuration.ShouldNotBeNull();
            _collector = Metrics.Collector = new CollectorConfiguration()
                .WriteTo.InfluxDB(configuration.Url, configuration.Database)
                .CreateCollector();
        }
        public void RegisterDepartmentFeedback(string department, int feedback)
        {
            var values = new Dictionary<string, object>
            {
                {"rating", feedback}
            };
            var tags = new Dictionary<string, string>
            {
                {"department", department}
            };

            _collector.Write("user_satisfaction", values, tags);
        }
    }
}
