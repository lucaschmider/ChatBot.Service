namespace ChatBot.MessageInterpreter.DialogFlow
{
    /// <summary>
    ///     Wraps the configuration for the dialog flow message interpreter
    /// </summary>
    public class DialogFlowConfiguration
    {
        /// <summary>
        ///     The key at which the configuration is located
        /// </summary>
        public static string SectionKey => "DialogFlow";

        /// <summary>
        ///     String content of the service account file
        /// </summary>
        public string ServiceAccountJson { get; set; }
    }
}