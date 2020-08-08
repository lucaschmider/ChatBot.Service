using ChatBot.Repository.Contracts.Models;

namespace ChatBot.Repository.MongoDb.Models
{
    /// <inheritdoc />
    public class UserDetails : IUserDetails
    {
        public string Uid { get; set; }
        public bool IsAdmin { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
    }
}