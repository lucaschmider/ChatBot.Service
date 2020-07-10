const { Router } = require("express");

const router = Router();

router.get("/", async (req, res) => {
  const data = [
    {
      message: "Das ist eine Nachricht vom Server",
      timestamp: Date.now()
    },
    {
      message: "Das ist eine andere Nachricht vom Server",
      timestamp: Date.now()
    }
  ];
  res.send(data);
});

module.exports = {
  ChatController: router
};
