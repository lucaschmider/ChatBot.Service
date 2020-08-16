using System.Collections.Generic;

namespace ChatBot.Service.Models
{
    /// <summary>
    ///     Represents the request that is sent when one requests the possible questions they could ask
    /// </summary>
    public class PossibleQuestionsResponse
    {
        /// <summary>
        ///     The possible definition types
        /// </summary>
        public IEnumerable<string> DefinitionTypes { get; set; }

        /// <summary>
        ///     The possible keywords
        /// </summary>
        public IEnumerable<string> Keywords { get; set; }
    }
}