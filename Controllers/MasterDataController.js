const chalk = require("chalk");
const { Router } = require("express");

class MasterDataController {
  /**
   * Returns a router instance representing the StatisticsController
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.get("/departments", MasterDataController.GetDepartments);
    return router;
  }

  /**
   * Returns user satifaction
   * @param {Request} req
   * @param {Response} res
   */
  static GetDepartments(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }
    res.json([
      "Human Resources",
      "External Sales",
      "IT-Department",
      "Key Accounts",
      "Manufacturing Radar",
      "Manufacturing Capacitive"
    ]);
  }
}

module.exports = { MasterDataController };
