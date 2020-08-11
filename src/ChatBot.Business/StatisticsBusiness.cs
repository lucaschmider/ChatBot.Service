using System.Threading.Tasks;
using ChatBot.Business.Contracts.Statistics;
using ChatBot.Repository.Contracts;
using ChatBot.StatisticsProvider.Contract;
using Shouldly;

namespace ChatBot.Business
{
    /// <inheritdoc />
    public class StatisticsBusiness : IStatisticsBusiness
    {
        private readonly IStatisticsProvider _statisticsProvider;
        private readonly IUserRepository _userRepository;

        public StatisticsBusiness(IUserRepository userRepository, IStatisticsProvider statisticsProvider)
        {
            userRepository.ShouldNotBeNull();
            statisticsProvider.ShouldNotBeNull();

            _userRepository = userRepository;
            _statisticsProvider = statisticsProvider;
        }

        public async Task RegisterUserFeedback(string userId, int feedback)
        {
            var feedbackUser = await _userRepository
                .GetUserDetailsAsync(userId)
                .ConfigureAwait(false);

            _statisticsProvider.RegisterDepartmentFeedback(feedbackUser.Department, feedback);
        }
    }
}