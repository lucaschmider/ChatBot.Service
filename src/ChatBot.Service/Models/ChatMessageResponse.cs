using System;

namespace ChatBot.Service.Models
{
    /// <summary>
    ///     Represents a text message
    /// </summary>
    public class ChatMessageResponse
    {
        /// <summary>
        ///     The id of the user the message is sent to
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        ///     The content of the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     The time the message was sent
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Indicates whether this is the las message of the conversation
        /// </summary>
        public bool ConversationFinished { get; set; }
    }
}