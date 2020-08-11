using System.Collections.Generic;

namespace ChatBot.StatisticsProvider.Influx.Models
{
    public class InfluxDataResponse
    {
        public IEnumerable<InfluxStatementResponse> Results { get; set; }
    }
}