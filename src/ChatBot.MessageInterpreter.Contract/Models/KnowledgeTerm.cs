using System.Collections.Generic;

namespace ChatBot.MessageInterpreter.Contract.Models
{
    /// <summary>
    ///     Represents a term representation as used for message interpretation
    /// </summary>
    public class KnowledgeTerm
    {
        /// <summary>
        ///     The keyword of the term
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        ///     Alternative words describing the term
        /// </summary>
        public IEnumerable<string> Synonyms { get; set; }
    }
}