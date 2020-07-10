const { StatisticsRepository } = require("../Repository/StatisticsRepository.js");

class StatisticsBusiness {
  GetUserSatisfactionAsync = async () => {
    const data = await StatisticsRepository.GetStatisticsAsync();
    return data;
  };
}

module.exports = { StatisticsBusiness };
