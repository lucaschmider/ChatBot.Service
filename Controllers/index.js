const { Router } = require("express");
const { StatisticsController } = require("./StatisticsController");
const { ChatController } = require("./ChatController");
const { UserController } = require("./UserController");
const authMiddleware = require("../Middleware/AuthMiddleware");
const router = Router();

router.use("/Statistics", authMiddleware, StatisticsController);
router.use("/Chat", authMiddleware, ChatController);
router.use("/user", authMiddleware, UserController);

module.exports = router;
