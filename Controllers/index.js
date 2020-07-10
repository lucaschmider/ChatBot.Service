const { Router } = require("express");
const { StatisticsController } = require("./StatisticsController");
const { ChatController } = require("./ChatController");
const { UserController } = require("./UserController");
const authMiddleware = require("../Middleware/AuthMiddleware");

class Controllers {
  /**
   * Returns a router instance representing all controllers
   * @returns {Router}
   */
  static getRouter() {
    const router = Router();

    router.use("/statistics", authMiddleware, StatisticsController.getRouter());
    router.use("/chat", authMiddleware, ChatController.getRouter());
    router.use("/user", authMiddleware, UserController.getRouter());
    return router;
  }
}

module.exports = { Controllers };
