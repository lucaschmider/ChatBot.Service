namespace ChatBot.Repository.Contracts.Models
{
    /// <summary>
    ///     Represents a single users details
    /// </summary>
    public class User
    {
        /// <summary>
        ///     The id of the user described
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        ///     Indicates whether the user has administrative access
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        ///     The fullname of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The department of the user
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        ///     The email of the user
        /// </summary>
        public string Email { get; set; }
    }
}