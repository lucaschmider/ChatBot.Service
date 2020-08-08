using System.Threading.Tasks;
using ChatBot.Repository.Contracts.Models;

namespace ChatBot.Repository.Contracts
{
    /// <summary>
    ///     Provides methods to manage user data
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        ///     Returns details about the requested user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IUserDetails> GetUserDetailsAsync(string userId);
    }
}