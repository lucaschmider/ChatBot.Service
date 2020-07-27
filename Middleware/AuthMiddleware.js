const admin = require("firebase-admin");
const { UserRepository } = require("../Repository/UserRepository");
const chalk = require("chalk");

class AuthMiddleware {
  /**
   * Extracts the Bearer access token & stores it in the request object
   * @param {import("express").Request} req
   * @param {import("express").Response} res
   * @param {import("express").NextFunction} next
   * @private
   */
  static getAuthToken(req, res, next) {
    if (req.headers.authorization && req.headers.authorization.split(" ")[0] === "Bearer") {
      req.authToken = req.headers.authorization.split(" ")[1];
    } else {
      req.authToken = null;
    }
    next();
  }

  /**
   * Verifies that the access token provided is valid. Rejects the incoming request for invalid tokens.
   * @param {import("express").Request} req
   * @param {import("express").Response} res
   * @param {import("express").NextFunction} next
   */
  static verifyIdToken(req, res, next) {
    AuthMiddleware.getAuthToken(req, res, async () => {
      try {
        const { authToken } = req;
        const userInfo = await admin.auth().verifyIdToken(authToken);
        const userData = await UserRepository.GetUserDataForUserAsync(userInfo.uid);
        if (!userData) {
          console.warn(chalk.yellow("No UserData found for authenticated user:\n", JSON.stringify(userInfo)));
          return res.status(401).send({ error: "You are not authorized to make this request" });
        }

        req.userData = userData;
        return next();
      } catch (e) {
        AuthMiddleware.handleError(e);
        return res.status(401).send({ error: "You are not authorized to make this request" });
      }
    });
  }

  /**
   * Handles upcoming errors
   * @param {Object} error
   * @private
   */
  static handleError(error) {
    console.log(error);
    console.log(chalk.yellow(`Authentification error: ${error.errorInfo.code}`));
  }
}

module.exports = { AuthMiddleware };
