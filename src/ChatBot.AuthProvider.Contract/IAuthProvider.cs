using System.Threading.Tasks;

namespace ChatBot.AuthProvider.Contract
{
    /// <summary>
    ///     Provides methods to create and delete users
    /// </summary>
    public interface IAuthProvider
    {
        /// <summary>
        ///     Creates a new user with the provided credentials. Returns the user id of the new user.
        /// </summary>
        /// <param name="username">The username of the new user</param>
        /// <param name="password">The password of the new user</param>
        /// <returns>The user id of the new user</returns>
        Task<string> CreateUserWithUsernameAndPasswordAsync(string username, string password);

        /// <summary>
        ///     Deletes the specified user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUserAsync(string userId);
    }
}