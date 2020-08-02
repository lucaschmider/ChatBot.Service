const admin = require("firebase-admin");
const { UserRepository } = require("../Repository/UserRepository");
const chalk = require("chalk");
class UserBusiness {
  /**
   * Validates and creates the specified user
   * @param {object} user
   * @param {string} user.email
   * @param {string} user.password
   * @param {string} user.department
   * @param {string} user.name
   * @param {boolean} user.isAdmin
   */
  static async CreateUserAsync(user) {
    try {
      const auth = admin.auth();

      if (!user.email || !user.password || !user.department || !user.name) {
        throw new Error({ code: "schema/inclomplete-data" });
      }

      const userRecord = await auth.createUser({
        displayName: user.name,
        email: user.email,
        password: user.password
      });

      const userData = {
        isAdmin: user.isAdmin,
        department: user.department,
        name: user.name,
        uid: userRecord.uid,
        email: user.email
      };

      await UserRepository.CreateUserAsync(userData);

      return {
        user: {
          uid: userRecord.uid,
          name: user.name,
          isAdmin: user.isAdmin,
          department: user.department,
          email: user.email
        },
        error: false,
        reason: null
      };
    } catch (error) {
      console.log("Business Error: " + error);
      if (error.code == "auth/email-already-exists") {
        return {
          error: true,
          reason: error.code,
          user: null
        };
      }

      if (error.code == "schema/incomplete-data") {
        return {
          error: true,
          reason: error.code,
          user: null
        };
      }

      console.log(
        chalk.red("Unexpected error occured while creating a new user:\n", error, "\n", JSON.stringify(user))
      );
      throw new Error(error);
    }
  }
}

module.exports = { UserBusiness };
