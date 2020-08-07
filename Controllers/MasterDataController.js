const chalk = require("chalk");
const { Router } = require("express");
const { MasterDataRepository } = require("../Repository/MasterDataRepository");
const { MasterDataBusiness } = require("../Business/MasterDataBusiness");
const { DialogFlowService } = require("../Services/DialogFlowService");
class MasterDataController {
  /**
   * Returns a router instance representing the StatisticsController
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.get("/data/departments", MasterDataController.GetDepartmentsAsync);
    router.post("/data/departments", MasterDataController.CreateDepartmentAsync);
    router.delete("/data/departments/:departmentId", MasterDataController.DeleteDepartmentAsync);
    router.get("/data/knowledge", MasterDataController.GetKnowledgeAsync);
    router.post("/data/knowledge", MasterDataController.CreateKnowledgeAsync);
    router.delete("/data/knowledge/:Term", MasterDataController.DeleteKnowledgeAsync);
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

    const departments = await MasterDataRepository.GetAllData("departments");
    res.json(departments);
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

    try {
      const result = await MasterDataBusiness.CreateKnowledgeAsync(req.body);

      if (result.error) {
        res.status(400).send(result);
        console.log(chalk.yellow("Bad request while creating knowledge:\n", JSON.stringify(result)));
        return;
      }

      const createdData = {
        name: req.body.name,
        keywords: [req.body.name, ...req.body.synonyms],
        definitiontype: req.body.definitiontype,
        description: req.body.description
      };
      res.json(createdData);
    } catch (error) {
      res.status(500).send();
      console.log(chalk.red("Unexpected error occured while creating knowledge:\n", error));
    }
  }

  /**
   * Deletes the knowledge about the specified Term
   * @param {Request} req
   * @param {Response} res
   */
  static async DeleteKnowledgeAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }

    try {
      const dialogFlowService = await DialogFlowService.getInstance();
      await Promise.all([
        MasterDataRepository.DeleteData("knowledge", { keyword: req.params.Term }),
        dialogFlowService.deleteKnowledge(req.params.Term)
      ]);

      res.status(204).send();
    } catch (error) {
      res.status(500).send();
      console.log(chalk.red("Unexpected error occured while deleting knowledge:\n", error));
    }
  }

  /**
   * Deletes a department
   * @param {Request} req
   * @param {Response} res
   */
  static async DeleteDepartmentAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }

    try {
      if (!req.params.departmentId) {
        res.status(400).send();
        return;
      }

      await MasterDataRepository.DeleteDataById("departments", req.params.departmentId);
      res.status(204).send();
    } catch (error) {
      res.status(500).send();
      console.log(chalk.red("Unexpected error occured while deleting department:\n", error));
    }
  }

  /**
   * Creates a department
   * @param {Request} req
   * @param {Response} res
   */
  static async CreateDepartmentAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      return;
    }

    try {
      if (typeof req.body.departmentName !== "string") {
        res.status(400).send();
        return;
      }

      const result = await MasterDataRepository.InsertCollectionData("departments", {
        departmentName: req.body.departmentName
      });
      const response = { ...req.body, _id: result.insertedId };
      res.send(response);
    } catch (error) {
      res.status(500).send();
      console.log(chalk.red("Unexpected error occured while creating department:\n", error));
    }
  }
}

module.exports = { MasterDataController };
