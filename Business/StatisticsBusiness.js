const statisticsRepository = require("../Repository/StatisticsRepository.js");

const GetUserSatisfactionAsync = async () => {
  const data = await statisticsRepository.GetStatisticsAsync();
  return data;
};

module.exports = {
  GetUserSatisfactionAsync: GetUserSatisfactionAsync,
};
