const { ChatRepository } = require("../Repository/ChatRepository");

class ChatBusiness {
  static async ReadMessagesOfUserAsync(userId) {
    return await ChatRepository.GetMessagesForUserAsync(userId);
  }
}

module.exports = { ChatBusiness };
