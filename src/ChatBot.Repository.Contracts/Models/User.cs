namespace ChatBot.Repository.Contracts.Models
{
    /// <summary>
    ///     Represents a single users details
    /// </summary>
    public interface IUserDetails
    {
        /// <summary>
        ///     The id of the user described
        /// </summary>
        string Uid { get; }

        /// <summary>
        ///     Indicates whether the user has administrative access
        /// </summary>
        bool IsAdmin { get; }

        /// <summary>
        ///     The fullname of the user
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The department of the user
        /// </summary>
        string Department { get; }

        /// <summary>
        ///     The email of the user
        /// </summary>
        string Email { get; }
    }
}