const { StatisticsRepository } = require("../Repository/StatisticsRepository.js");
const { UserRepository } = require("../Repository/UserRepository");

class StatisticsBusiness {
  static async GetUserSatisfactionAsync() {
    const data = await StatisticsRepository.GetStatisticsAsync();
    return data;
  }

  static async ProccessFeedbackAsync(userId, rating) {
    const userData = await UserRepository.GetUserDataForUserAsync(userId);
    const department = userData.department;

    await StatisticsRepository.RegisterFeedback(rating, department);
  }
}

module.exports = { StatisticsBusiness };
