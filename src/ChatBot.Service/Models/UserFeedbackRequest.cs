namespace ChatBot.Service.Models
{
    /// <summary>
    ///     Represents the request that is sent to assess a conversation
    /// </summary>
    public class UserFeedbackRequest
    {
        /// <summary>
        ///     The rating the user gives the conversation
        /// </summary>
        public int Rating { get; set; }
    }
}