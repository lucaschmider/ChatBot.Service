const { Router } = require("express");

class UserController {
  /**
   * Returns a router instance representing the UserController
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.get("/details", UserController.GetUserInformationAsync);
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
}

module.exports = { UserController };
