namespace ChatBot.Business.Contracts.User.Models
{
    /// <summary>
    ///     Represents an existing user
    /// </summary>
    public class UserModel
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

        /// <summary>
        ///     The password of the user
        /// </summary>
        public string Password { get; set; }
    }
}