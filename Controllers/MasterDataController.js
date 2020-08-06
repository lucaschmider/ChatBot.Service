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
    router.get("/data/knowledge", MasterDataController.GetKnowledgeAsync);
    router.post("/data/knowledge", MasterDataController.CreateKnowledgeAsync);
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
  static async GetKnowledgeAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }

    try {
      const knowledgebase = await MasterDataBusiness.GetKeywordsAsync();
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

  /**
   * Creates a new knowledge
   * @param {Request} req
   * @param {Response} res
   */
  static async CreateKnowledgeAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }

    // try {
    const result = await MasterDataBusiness.CreateKnowledgeAsync(req.body);

    if (result.error) {
      res.status(400).send(result.reason);
      return;
    }

    res.status(204).send();
    // } catch (error) {
    //   res.status(500).send();
    //   console.log(chalk.red("Unexpected error occured while creating knowledge:\n", error));
    // }
  }
}

module.exports = { MasterDataController };
