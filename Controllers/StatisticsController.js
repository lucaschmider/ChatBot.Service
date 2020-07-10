const { Router } = require("express");
const statisticsBusiness = require("../Business/StatisticsBusiness");

class StatisticsController {
  /**
   * Returns a router instance representing the StatisticsController
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.get("/", StatisticsController.GetUserSatisfactionAsync);
    return router;
  }

  /**
   * Returns user satifaction
   * @param {Request} req
   * @param {Response} res
   */
  static async GetUserSatisfactionAsync(req, res) {
    const data = await statisticsBusiness.GetUserSatisfactionAsync();
    res.send(data);
  }
}

module.exports = { StatisticsController };
