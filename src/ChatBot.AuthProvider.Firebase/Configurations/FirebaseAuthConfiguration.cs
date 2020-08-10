namespace ChatBot.AuthProvider.Firebase.Configurations
{
    /// <summary>
    ///     Wraps the configuration of the firebase auth module
    /// </summary>
    public class FirebaseAuthConfiguration
    {
        /// <summary>
        ///     The key at which the konfiguration is located
        /// </summary>
        public static string SectionKey => "Firebase";

        /// <summary>
        ///     The id of the project to use
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        ///     A json string representing the service account file
        /// </summary>
        public string ServiceAccountJson { get; set; }
    }
}