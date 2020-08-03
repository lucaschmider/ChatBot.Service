const admin = require("firebase-admin");
const chalk = require("chalk");
const { UserRepository } = require("../Repository/UserRepository");

class DataIntegrityService {
  /**
   * Ensures, that required data is present.
   */
  static async EnsureIntegrity() {
    await this.EnsureUserDataIntegrity();
  }

  /**
   * Ensures, that an administrative user is present.
   * @private
   */
  static async EnsureUserDataIntegrity() {
    const allUsers = await UserRepository.GetAllUsersAsync();
    const firebaseUsers = (await admin.auth().listUsers()).users;
    const presentNecessaryUsers = allUsers.filter((user) => {
      if (!user || !user.uid) {
        return false;
      }
      if (!firebaseUsers.find((firebaseUser) => firebaseUser.uid == user.uid)) {
        return false;
      }
      return true;
    });

    if (presentNecessaryUsers.length == 0) {
      console.log(chalk.redBright("No administrative user present. Creating default user"));

      if (!allUsers.find((user) => user.uid == "4ZsA71pqHvRL2yKyvHiWK3TGGo02")) {
        await UserRepository.CreateUserAsync({
          department: "IT-Department",
          isAdmin: true,
          name: "Admin User",
          uid: "4ZsA71pqHvRL2yKyvHiWK3TGGo02",
          email: "max.mustermann@gmail.com"
        });
      }

      if (!firebaseUsers.find((firebaseUser) => firebaseUser.uid == "4ZsA71pqHvRL2yKyvHiWK3TGGo02")) {
        await admin.auth().createUser({
          uid: "4ZsA71pqHvRL2yKyvHiWK3TGGo02",
          displayName: "Admin User",
          email: "max.mustermann@gmail.com",
          password: "12345678"
        });
      }
    }
  }
}

module.exports = { DataIntegrityService };
