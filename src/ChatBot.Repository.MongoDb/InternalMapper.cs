using ChatBot.Repository.Contracts.Models;
using ChatBot.Repository.MongoDb.Models;

namespace ChatBot.Repository.MongoDb
{
    public static class InternalMapper
    {
        public static User Map(this InternalUser user)
        {
            return new User
            {
                Department = user.Department,
                IsAdmin = user.IsAdmin,
                Name = user.Name,
                Email = user.Email,
                Uid = user.Uid
            };
        }

        public static InternalUser Map(this User user)
        {
            return new InternalUser
            {
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                Name = user.Name,
                Department = user.Department,
                Uid = user.Uid
            };
        }
    }
}