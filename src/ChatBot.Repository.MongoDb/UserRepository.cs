using System.Threading.Tasks;
using ChatBot.Repository.Contracts;
using ChatBot.Repository.Contracts.Models;
using ChatBot.Repository.MongoDb.Models;

namespace ChatBot.Repository.MongoDb
{
    /// <inheritdoc />
    public class UserRepository : IUserRepository
    {
        /// <inheritdoc />
        public async Task<IUserDetails> GetUserDetailsAsync(string userId)
        {
            return await Task.FromResult(new UserDetails
            {
                Uid = userId,
                Department = "IT-Department",
                Email = "max.mustermann@gmail.com",
                IsAdmin = true,
                Name = "Max Mustermann"
            });
        }
    }
}