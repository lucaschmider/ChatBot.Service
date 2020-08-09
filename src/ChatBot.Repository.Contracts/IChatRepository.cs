using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatBot.Repository.Contracts.Models;

namespace ChatBot.Repository.Contracts
{
    public interface IChatRepository
    {
        /// <summary>
        ///     Sends the specified message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageAsync(ChatMessage message);

        Task<IEnumerable<ChatMessage>> GetChatMessagesForUserAsync(string userId);
    }
}
