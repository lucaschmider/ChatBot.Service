const { ChatRepository } = require("../Repository/ChatRepository");
const { DialogFlowService } = require("../Services/DialogFlowService");

class ChatBusiness {
  static async ReadMessagesOfUserAsync(userId) {
    const newMessages = (await ChatRepository.GetMessagesForUserAsync(userId)).sort(
      (a, b) => a.create_date - b.create_date
    );

    if (newMessages.length === 0) return newMessages;
    const oldestTimestamp = newMessages[0].create_date.getTime();
    await ChatRepository.DeleteMessagesForUserOlderThan(userId, oldestTimestamp);
    return newMessages.map((message) => {
      return { message: message.message, timestamp: message.create_date.getTime() };
    });
  }

  static async HandleMessageAsync(userId, messageText) {
    console.log({ userId, messageText });
    const answer = await DialogFlowService.HandleMessage(userId, messageText);
    console.log(answer);

    if (answer && answer.answerText.length > 0) {
      await ChatRepository.CreateMessageAsync(userId, answer.answerText);
    }

    if (answer.isCompleted) {
      await ChatRepository.CreateMessageAsync(
        userId,
        `Ok, ein ${answer.parameters.keyword} nach ${answer.parameters.definitiontype} ist [...].`
      );
    }
  }
}

module.exports = { ChatBusiness };
