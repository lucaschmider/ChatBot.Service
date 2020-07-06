const { Router } = require("express");

const router = Router();

router.get("/", async (req, res) => {
  res.send({ status: "Ok" });
});

module.exports = {
  ChatController: router
};
