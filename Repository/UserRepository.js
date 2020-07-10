const UserModel = require("./Models/UserModel");

module.exports = {
  GetUserDataForUserAsync: async (uid) => {
    const doc = await UserModel.findOne({ uid });
    return doc.toObject();
  }
};
