﻿namespace ChatBot.StatisticsProvider.Influx
{
    /// <summary>
    ///     Wraps the configuration for connecting to influx db
    /// </summary>
    public class InfluxDbConfiguration
    {
        /// <summary>
        ///     The Url of the servers api
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     The database to use
        /// </summary>
        public string Database { get; set; }
    }
}