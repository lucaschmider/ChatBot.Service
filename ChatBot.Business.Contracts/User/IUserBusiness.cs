using System.Threading.Tasks;
using ChatBot.Business.Contracts.User.Models;

namespace ChatBot.Business.Contracts.User
{
    public interface IUserBusiness
    {
        /// <summary>
        ///     Creates a new user alongside with meta information
        /// </summary>
        /// <param name="createUserRequest"></param>
        /// <returns></returns>
        Task<UserModel> CreateUserAsync(UserModel createUserRequest);

        /// <summary>
        ///     Returns a flag indicating whether the user has administrative privileges
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CheckAdminPrivilegesAsync(string userId);
    }
}