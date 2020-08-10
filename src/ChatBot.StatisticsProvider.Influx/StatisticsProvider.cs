using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ChatBot.StatisticsProvider.Contract;
using ChatBot.StatisticsProvider.Contract.Exceptions;
using ChatBot.StatisticsProvider.Contract.Models;
using ChatBot.StatisticsProvider.Influx.Models;
using InfluxDB.Collector;
using Newtonsoft.Json;
using Shouldly;

namespace ChatBot.StatisticsProvider.Influx
{
    public class StatisticsProvider : IStatisticsProvider
    {
        private const string Query = "SELECT%20mean%28%22rating%22%29%20FROM%20%22user_satisfaction%22%20GROUP%20BY%20time%2830m%29%2C%20%22department%22%20FILL%28previous%29";

        private readonly MetricsCollector _collector;
        private readonly InfluxDbConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public StatisticsProvider(InfluxDbConfiguration configuration)
        {
            configuration.ShouldNotBeNull();

            _configuration = configuration;
            _collector = Metrics.Collector = new CollectorConfiguration()
                .WriteTo.InfluxDB(configuration.Url, configuration.Database)
                .CreateCollector();
            _httpClient = new HttpClient();
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

        public async Task<IEnumerable<DepartmentReport>> GetFeedbackAsync()
        {
            var httpResponse =
                await _httpClient.GetAsync($"{_configuration.Url}/query?db={_configuration.Database}&q={Query}");

            if (!httpResponse.IsSuccessStatusCode) throw new StatisticsRetrievalException();
            var influxResponse =
                JsonConvert.DeserializeObject<InfluxDataResponse>(await httpResponse.Content.ReadAsStringAsync());

            return influxResponse.Results
                .First()
                .Series.Select(series => new DepartmentReport
                {
                    Department = series.Tags["department"],
                    RatingSlices = series.Values.Select(value =>
                    {
                        var slice = value as object[] ?? value.ToArray();
                        return new RatingSlice
                        {
                            Timestamp = (DateTime) slice[0],
                            Value = Convert.ToSingle( slice[1])
                        };
                    })
                });
        }
    }
}