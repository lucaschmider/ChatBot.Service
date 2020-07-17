const chalk = require("chalk");
const { Router } = require("express");
const { StatisticsBusiness } = require("../Business/StatisticsBusiness");

class StatisticsController {
  /**
   * Returns a router instance representing the StatisticsController
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.get("/", StatisticsController.GetUserSatisfactionAsync);
    router.post("/feedback", StatisticsController.CreateFeedbackAsync);
    return router;
  }

  /**
   * Returns user satifaction
   * @param {Request} req
   * @param {Response} res
   */
  static async GetUserSatisfactionAsync(req, res) {
    const data = await StatisticsBusiness.GetUserSatisfactionAsync();
    res.send(data);
  }

  /**
   * Saves anonymous user satisfaction data
   * @param {Request} req
   * @param {Response} res
   */
  static async CreateFeedbackAsync(req, res) {
    try {
      await StatisticsBusiness.ProccessFeedbackAsync(req.userData.uid, req.body.rating);
      res.status(204).send();
    } catch (error) {
      console.log(chalk.red("An unhandled error occured:\n"), error);
      res.status(500).send();
    }
  }
}

module.exports = { StatisticsController };
