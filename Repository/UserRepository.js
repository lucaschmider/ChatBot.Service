const UserModel = require("./Models/UserModel");

class UserRepository {
  /**
   *  Returns information associated with the provided userId
   * @param {string} uid
   * @returns {Object} UserData of the specified userId
   */
  static async GetUserDataForUserAsync(uid) {
    const doc = await UserModel.findOne({ uid });
    return doc && doc.toObject();
  }

  /**
   *  Returns a list of all users
   */
  static async GetAllUsersAsync() {
    const docs = await UserModel.find();
    return docs && docs.map((doc) => doc.toObject());
  }

  /**
   *
   * @param {object} User The User to create
   * @param {string} User.uid The uid of the new user
   * @param {boolean} User.isAdmin Flag which determines if a user has advanced privileges
   * @param {string} User.department The department of the new user
   * @param {string} User.name The full name of the new user
   */
  static async CreateUserAsync({ uid, isAdmin, department, name }) {
    const doc = await UserModel.create({
      uid,
      isAdmin,
      department,
      name
    });

    return doc && doc.toObject();
  }
}
module.exports = { UserRepository };
