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

        /// <summary>
        ///     The id of the dialog flow project
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        ///     The guid identifier for the dialog flow entity type keyword
        /// </summary>
        public string KeywordsEntityTypeGuid { get; set; }

        /// <summary>
        ///     The guid identifier for the dialog flow entity type definition type
        /// </summary>
        public string DefinitionTypeEntityTypeGuid { get; set; }

        /// <summary>
        ///     The name of the intent used to define a term
        /// </summary>
        public string DefineIntentName { get; set; }
    }
}