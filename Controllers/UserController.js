const { Router } = require("express");

const router = Router();

router.get("/details", async (req, res) => {
  const user = {
    uid: req.userData.uid,
    isAdmin: req.userData.isAdmin,
    name: req.userData.name
  };

  res.json(user);
});

module.exports = {
  UserController: router
};
