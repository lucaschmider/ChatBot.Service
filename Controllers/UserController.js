const chalk = require("chalk");
const admin = require("firebase-admin");
const { Router } = require("express");
const { UserRepository } = require("../Repository/UserRepository");
const { UserBusiness } = require("../Business/UserBusiness");
class UserController {
  /**
   * Returns a router instance representing the UserController
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.get("/details", UserController.GetUserInformationAsync);
    router.post("/", UserController.CreateUserAsync);
    router.get("/", UserController.GetUsersAsync);
    router.delete("/:uid", UserController.DeleteUserAsync);
    return router;
  }

  /**
   * Returns information about the requesting user.
   * @param {Request} req
   * @param {Response} res
   */
  static async GetUserInformationAsync(req, res) {
    const user = {
      uid: req.userData.uid,
      isAdmin: req.userData.isAdmin,
      name: req.userData.name
    };

    res.json(user);
  }

  /**
   * Returns a list of users.
   * @param {Request} req
   * @param {Response} res
   */
  static async GetUsersAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      console.log(chalk.yellow("Unauthorized attempt to get user data."));
      return;
    }

    const users = await UserRepository.GetAllUsersAsync();
    const result = users.map((user) => {
      return {
        uid: user.uid,
        name: user.name,
        isAdmin: user.isAdmin,
        department: user.department,
        email: user.email
      };
    });
    res.json(result);
  }

  /**
   * Creates a new user.
   * @param {Request} req
   * @param {Response} res
   */
  static async CreateUserAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      console.log(chalk.yellow("Unauthorized attempt to create a new user.\n", req.body));
      return;
    }

    try {
      const createUserResult = await UserBusiness.CreateUserAsync(req.body);
      console.log(createUserResult);

      if (createUserResult.error) {
        res.status(400).json({ reason: createUserResult.reason });
        return;
      }

      res.json(createUserResult.user);
    } catch (error) {
      console.log(chalk.yellow("Unexpected error occured while creating new user:\n", error));
      res.status(500).send();
    }
  }

  /**
   * Deletes a user
   * @param {Request} req
   * @param {Response} res
   */
  static async DeleteUserAsync(req, res) {
    if (!req.userData.isAdmin) {
      res.status(401).send();
      console.log(chalk.yellow(`Unauthorized attempt to delete user ${req.params.uid}.\n`, req.body));
      return;
    }

    try {
      await admin.auth().deleteUser(req.params.uid);
      await UserRepository.DeleteUserAsync(req.params.uid);
      res.status(204).send();
    } catch (error) {
      console.log(chalk.yellow("Unexpected error occured while creating new user:\n", error));
      res.status(500).send();
    }
  }
}

module.exports = { UserController };
