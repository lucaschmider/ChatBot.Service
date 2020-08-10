using System.Threading.Tasks;

namespace ChatBot.Business.Contracts.Statistics
{
    /// <summary>
    ///     Provides methods to anonymize, retrieve and collect user feedback 
    /// </summary>
    public interface IStatisticsBusiness
    {
        /// <summary>
        ///     Anonymizes the feedback and saved it
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="feedback"></param>
        /// <returns></returns>
        Task RegisterUserFeedback(string userId, int feedback);
    }
}