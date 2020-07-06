const { Router } = require("express");
const statisticsBusiness = require("../Business/StatisticsBusiness");

const router = Router();

router.get("/", async (req, res) => {
  const data = await statisticsBusiness.GetUserSatisfactionAsync();
  res.send(data);
});

module.exports = {
  StatisticsController: router
};
