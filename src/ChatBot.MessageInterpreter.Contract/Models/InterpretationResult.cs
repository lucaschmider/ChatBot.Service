using System.Collections.Generic;

namespace ChatBot.MessageInterpreter.Contract.Models
{
    /// <summary>
    ///     Wraps the product of a message interpretation
    /// </summary>
    public class InterpretationResult
    {
        /// <summary>
        ///     The intent that was detected
        /// </summary>
        public IntentType DetectedIntent { get; set; }

        /// <summary>
        ///     Indicates whether all parameters are present
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        ///     A question to send to the user in order to get all required information
        /// </summary>
        public string AnswerString { get; set; }

        /// <summary>
        ///     The parameters detected in the conversation
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }
    }
}