const { Router } = require("express");
const { StatisticsController } = require("./StatisticsController");
const { ChatController } = require("./ChatController");
const { UserController } = require("./UserController");
const { AuthMiddleware } = require("../Middleware/AuthMiddleware");

class Controllers {
  /**
   * Returns a router instance representing all controllers
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();
    router.use("/statistics", AuthMiddleware.verifyIdToken, StatisticsController.getRouter());
    router.use("/chat", AuthMiddleware.verifyIdToken, ChatController.getRouter());
    router.use("/user", AuthMiddleware.verifyIdToken, UserController.getRouter());
    return router;
  }
}

module.exports = { Controllers };
