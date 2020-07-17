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

    const ratingData = {
      rating,
      department
    };

    console.log(ratingData);
    throw new Error("Not implemented!");
  }
}

module.exports = { StatisticsBusiness };
