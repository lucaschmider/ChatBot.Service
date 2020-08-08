using System;
using System.Threading.Tasks;
using ChatBot.Business.Contracts.User;
using ChatBot.Business.Contracts.User.Models;

namespace ChatBot.Business
{
    public class UserBusiness : IUserBusiness
    {
        public Task<UserModel> CreateUserAsync(UserModel createUserRequest)
        {
            throw new NotImplementedException();
        }
    }
}