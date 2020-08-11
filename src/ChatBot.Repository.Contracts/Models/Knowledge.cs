namespace ChatBot.Repository.Contracts.Models
{
    /// <summary>
    ///     Represents the knowledge about a single term
    /// </summary>
    public class Knowledge
    {
        /// <summary>
        ///     The definition type of the knowledge (eg. reference)
        /// </summary>
        public string DefinitionType { get; set; }

        /// <summary>
        ///     The keyword of the knowledge
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        ///     The explanation of the term
        /// </summary>
        public string Description { get; set; }
    }
}