namespace ChatBot.Repository.Contracts.Models
{
    /// <summary>
    ///     Represents a text message
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        ///     The id of the user the message is sent to
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        ///     The content of the message
        /// </summary>
        public string Message { get; set; }
    }
}