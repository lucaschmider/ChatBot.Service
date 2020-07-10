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
}
module.exports = { UserRepository };
