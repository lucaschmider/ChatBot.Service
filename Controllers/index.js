const { Router } = require("express");
const { StatisticsController } = require("./StatisticsController");
const { ChatController } = require("./ChatController");
const router = Router();

router.use("/Statistics", StatisticsController);
router.use("/Chat", ChatController);

module.exports = router;
