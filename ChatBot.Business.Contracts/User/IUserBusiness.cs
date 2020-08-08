using System.Threading.Tasks;
using ChatBot.Business.Contracts.User.Models;

namespace ChatBot.Business.Contracts.User
{
    public interface IUserBusiness
    {
        Task<UserModel> CreateUserAsync(UserModel createUserRequest);
    }
}