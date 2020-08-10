using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBot.StatisticsProvider.Contract.Models;

namespace ChatBot.StatisticsProvider.Contract
{
    /// <summary>
    ///     Provides methods to collect and analyze user feedback
    /// </summary>
    public interface IStatisticsProvider
    {
        /// <summary>
        ///     Registers the feedback of a user
        /// </summary>
        /// <param name="department"></param>
        /// <param name="feedback"></param>
        void RegisterDepartmentFeedback(string department, int feedback);

        /// <summary>
        ///     Returns all available user feedback
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DepartmentReport>> GetFeedbackAsync();
    }
}