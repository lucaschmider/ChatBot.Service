using System.Threading.Tasks;

namespace ChatBot.Business.Contracts.Chat
{
    /// <summary>
    ///     Provides methods to handle chat messages
    /// </summary>
    public interface IChatBusiness
    {
        /// <summary>
        ///     Interprets a message and creates an answer
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task HandleMessageAsync(string message, string userId);
    }
}