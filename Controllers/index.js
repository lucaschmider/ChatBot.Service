const { Router } = require("express");
const { StatisticsController } = require("./StatisticsController");
const { ChatController } = require("./ChatController");

const authMiddleware = require("../Middleware/AuthMiddleware");
const router = Router();

router.use("/Statistics", authMiddleware, StatisticsController);
router.use("/Chat", authMiddleware, ChatController);

module.exports = router;
