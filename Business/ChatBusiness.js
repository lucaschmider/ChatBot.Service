const { ChatRepository } = require("../Repository/ChatRepository");

class ChatBusiness {
  static async ReadMessagesOfUserAsync(userId) {
    const newMessages = (await ChatRepository.GetMessagesForUserAsync(userId)).sort(
      (a, b) => a.create_date - b.create_date
    );

    if (newMessages.length === 0) return newMessages;
    const oldestTimestamp = newMessages[0].create_date.getTime();
    await ChatRepository.DeleteMessagesForUserOlderThan(userId, oldestTimestamp);
    return newMessages;
  }

  static async SendMessageAsync(userId, messageText) {
    await ChatRepository.CreateMessageAsync(userId, messageText);
  }
}

module.exports = { ChatBusiness };
