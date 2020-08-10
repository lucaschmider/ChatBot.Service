using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChatBot.StatisticsProvider.Influx.Models
{
    public class InfluxStatementResponse
    {
        [JsonProperty("statement_id")] public int StatementId { get; set; }

        public IEnumerable<InfluxSeries> Series { get; set; }
    }
}