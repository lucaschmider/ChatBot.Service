namespace ChatBot.MessageInterpreter.Contract.Models
{
    /// <summary>
    ///     The type of intent
    /// </summary>
    public enum IntentType
    {
        /// <summary>
        ///     The value used, if no other type matches
        /// </summary>
        DefaultFallback,

        /// <summary>
        ///     The users likes to get the definition of a term
        /// </summary>
        DefinitionIntent
    }
}