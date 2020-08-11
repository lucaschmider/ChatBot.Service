namespace ChatBot.Repository.MongoDb.Configurations
{
    /// <summary>
    ///     Wraps the configuration for a mongo db connection
    /// </summary>
    public class MongoDbConfiguration
    {
        /// <summary>
        ///     The key at which the configuration is located
        /// </summary>
        public static string SectionKey => "Mongo";

        /// <summary>
        ///     The connection string to use
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     The name of the database to use
        /// </summary>
        public string Database { get; set; }
    }
}