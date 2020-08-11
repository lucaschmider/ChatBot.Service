using System.Collections.Generic;
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
        Task<User> GetUserDetailsAsync(string userId);

        /// <summary>
        ///     Returns a list of all registered user meta data
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        ///     Writes user details to the database
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        Task CreateUserDetailsAsync(User details);

        /// <summary>
        ///     Deletes the associated meta information
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUserDetailsAsync(string userId);
    }
}