using System.Collections.Generic;

namespace ChatBot.Service.Models
{
    /// <summary>
    ///     Represents the request that is sent to create new knowledge
    /// </summary>
    public class CreateKnowledgeRequest
    {
        /// <summary>
        ///     The type of definition (eg. reference)
        /// </summary>
        public string DefinitionType { get; set; }

        /// <summary>
        ///     The explanation of the term
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The name of the term
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     All words should be recognized as this term
        /// </summary>
        public IEnumerable<string> Synonyms { get; set; }
    }
}