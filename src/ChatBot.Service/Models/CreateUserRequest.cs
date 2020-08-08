namespace ChatBot.Business.Contracts.User.Models
{
    /// <summary>
    ///     Represents the request used to create user
    /// </summary>
    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}