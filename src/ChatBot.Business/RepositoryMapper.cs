using ChatBot.Business.Contracts.User.Models;
using ChatBot.Repository.Contracts.Models;

namespace ChatBot.Business
{
    public static class RepositoryMapper
    {
        public static UserModel Map(this User user)
        {
            return new UserModel
            {
                Department = user.Department,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                Name = user.Name,
                Uid = user.Uid
            };
        }

        public static User Map(this UserModel user)
        {
            return new User
            {
                Uid = user.Uid,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                Email = user.Email,
                Department = user.Department
            };
        }
    }
}