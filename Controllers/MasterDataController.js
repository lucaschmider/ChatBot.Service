const chalk = require("chalk");
const { Router } = require("express");
const { MasterDataRepository } = require("../Repository/MasterDataRepository");

class MasterDataController {
  /**
   * Returns a router instance representing the StatisticsController
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.get("/departments", MasterDataController.GetDepartmentsAsync);
    return router;
  }

  /**
   * Returns user satifaction
   * @param {Request} req
   * @param {Response} res
   */
  static async GetDepartmentsAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }

    const departments = await MasterDataRepository.GetAllData("departments");
    res.json(departments.map((department) => department.departmentName));
  }
}

module.exports = { MasterDataController };
