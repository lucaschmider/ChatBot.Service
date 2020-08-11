using System.Collections.Generic;

namespace ChatBot.Service.Models
{
    /// <summary>
    ///     Represents a single term
    /// </summary>
    public class KnowledgeResponse
    {
        // <summary>
        /// The name of the knowledge
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The type of definition (eg. reference)
        /// </summary>
        public string DefinitionType { get; set; }

        /// <summary>
        ///     The description of the knowledge
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The synonyms of the knowledge
        /// </summary>
        public IEnumerable<string> Keywords { get; set; }
    }
}