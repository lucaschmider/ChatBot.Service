namespace ChatBot.Service.Models
{
    /// <summary>
    ///     Represents a newly created user
    /// </summary>
    public class CreateUserResponse
    {
        /// <summary>
        ///     The id of the user
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        ///     The name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     The department of the user
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        ///     Indicates whether the user is an administrator
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}