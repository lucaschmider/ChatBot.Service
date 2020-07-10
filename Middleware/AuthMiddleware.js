const admin = require("firebase-admin");
const userRepository = require("../Repository/UserRepository");
const chalk = require("chalk");
const { auth } = require("firebase-admin");

const getAuthToken = (req, res, next) => {
  if (req.headers.authorization && req.headers.authorization.split(" ")[0] === "Bearer") {
    req.authToken = req.headers.authorization.split(" ")[1];
  } else {
    req.authToken = null;
  }
  next();
};

const verifyIdToken = (req, res, next) => {
  getAuthToken(req, res, async () => {
    try {
      const { authToken } = req;
      const userInfo = await admin.auth().verifyIdToken(authToken);
      const userData = await userRepository.GetUserDataForUserAsync(userInfo.uid);

      if (!userData) {
        console.warn(chalk.yellow("No UserData found for authenticated user:\n", JSON.stringify(userInfo)));
        return res.status(401).send({ error: "You are not authorized to make this request" });
      }

      req.userData = userData;
      return next();
    } catch (e) {
      handleError(e);
      return res.status(401).send({ error: "You are not authorized to make this request" });
    }
  });
};

const handleError = (error) => {
  console.log(chalk.yellow(`Authentification error: ${error.code}`));
};
module.exports = verifyIdToken;
