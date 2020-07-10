const statisticsRepository = require("../Repository/StatisticsRepository.js");

class StatisticsBusiness {
  GetUserSatisfactionAsync = async () => {
    const data = await statisticsRepository.GetStatisticsAsync();
    return data;
  };
}

module.exports = { StatisticsBusiness };
