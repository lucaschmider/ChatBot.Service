using System.Collections.Generic;

namespace ChatBot.StatisticsProvider.Influx.Models
{
    public class InfluxSeries
    {
        public string Name { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public IEnumerable<string> Columns { get; set; }
        public IEnumerable<IEnumerable<object>> Values { get; set; }
    }
}