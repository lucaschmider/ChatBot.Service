const MessageModel = require("./Models/MessageModel");

class ChatRepository {
  /**
   *  Creates a new message
   * @param {Object} obj
   * @param {string} obj.receipient The receipient of the message
   * @param {string} obj.message The text of the message
   */
  static async CreateMessageAsync({ receipient, message }) {
    const doc = await MessageModel.create({
      receipient: receipient,
      message: message
    });

    return doc.toObject();
  }

  /**
   * Returns an array of all unread messages for the user
   * @param {string} userId The userId to filter for
   */
  static async GetMessagesForUserAsync(userId) {
    return new Promise((resolve) =>
      setTimeout(() => {
        resolve([
          {
            message: "Das ist eine Nachricht vom Server",
            timestamp: Date.now()
          },
          {
            message: "Das ist eine andere Nachricht vom Server",
            timestamp: Date.now()
          }
        ]);
      }, 500)
    );
  }

  /**
   * Deletes all messages which are older than specified for the selected user
   * @param {string} userId  The userId to filter for
   * @param {Date} older_than The maximum latest date to delete
   */
  static async DeleteMessagesForUserOlderThan(userId, older_than) {
    await MessageModel.deleteMany({
      create_date: { $lte: older_than },
      receipient: userId
    });
  }
}
module.exports = { ChatRepository };
