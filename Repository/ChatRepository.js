const MessageModel = require("./Models/MessageModel");

module.exports = {
  CreateMessageAsync: async ({ receipient, message }) => {
    const doc = await MessageModel.create({
      receipient: receipient,
      message: message,
    });

    return doc.toObject();
  },
  GetMessagesForUserAsync: async (userId) => {
    const docs = await MessageModel.find({ receipient: userId }).sort([
      "create_date",
    ]);

    return docs;
  },
  DeleteMessagesForUserOlderThan: async (userId, older_than) => {
    await MessageModel.deleteMany({
      create_date: { $lte: older_than },
      receipient: userId,
    });
  },
};
