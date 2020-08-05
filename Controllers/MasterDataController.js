const chalk = require("chalk");
const { Router } = require("express");
const { MasterDataRepository } = require("../Repository/MasterDataRepository");
const { MasterDataBusiness } = require("../Business/MasterDataBusiness");
class MasterDataController {
  /**
   * Returns a router instance representing the StatisticsController
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.get("/data/departments", MasterDataController.GetDepartmentsAsync);
    router.get("/data/knowledgebase", MasterDataController.GetKnowledgebaseAsync);
    router.get("/scheme/:collection", MasterDataController.GetCollectionSchemeAsync);
    return router;
  }

  /**
   * Returns a list of registered departments
   * @param {Request} req
   * @param {Response} res
   */
  static async GetDepartmentsAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }

    const departments = await MasterDataRepository.GetAllData(req.params.collection);
    res.json(departments.map((department) => department.departmentName));
  }

  /**
   * Returns a list of known definitions
   * @param {Request} req
   * @param {Response} res
   */
  static async GetKnowledgebaseAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }

    try {
      await MasterDataBusiness.GetKnowledgebaseAsync();
      const knowledgebase = await MasterDataRepository.GetAllData("knowledge");
      res.json(knowledgebase);
    } catch (error) {
      console.log(chalk.yellow("Bad Request while getting knowledgebase:\n", error));
      res.status(400).send();
    }
  }

  /**
   * Returns the scheme for the specified collection
   * @param {Request} req
   * @param {Response} res
   */
  static async GetCollectionSchemeAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }

    try {
      const scheme = await MasterDataRepository.GetCollectionScheme(req.params.collection);
      res.json(scheme);
    } catch (error) {
      console.log(chalk.yellow("Bad Request while getting collection scheme:\n", error));
      res.status(400).send();
    }
  }
}

module.exports = { MasterDataController };
