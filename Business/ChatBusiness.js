const { ChatRepository } = require("../Repository/ChatRepository");
const { DialogFlowService } = require("../Services/DialogFlowService");
const { ConfigService } = require("../Services/ConfigService");

class ChatBusiness {
  static async ReadMessagesOfUserAsync(userId) {
    const newMessages = (await ChatRepository.GetMessagesForUserAsync(userId)).sort(
      (a, b) => a.create_date - b.create_date
    );

    if (newMessages.length === 0) return newMessages;
    const oldestTimestamp = newMessages[0].create_date.getTime();
    await ChatRepository.DeleteMessagesForUserOlderThan(userId, oldestTimestamp);
    return newMessages.map((message) => {
      return {
        message: message.message,
        timestamp: message.create_date.getTime(),
        conversationFinished: message.conversationFinished
      };
    });
  }

  static async HandleMessageAsync(userId, messageText) {
    const answer = await DialogFlowService.HandleMessage(userId, messageText);
    if (answer.isCompleted && answer.detectedIntent === ConfigService.loadedConfiguration.dialogflowIntent) {
      await this.AnswerQuestionAsync(userId, answer.parameters);
      return;
    }

    if (answer && answer.answerText.length > 0) {
      await ChatRepository.CreateMessageAsync(userId, answer.answerText);
      return;
    }
  }

  static async AnswerQuestionAsync(userId, parameters) {
    await ChatRepository.CreateMessageAsync(
      userId,
      `Ok, ein ${parameters.keyword} nach ${parameters.definitiontype} ist [...].`,
      true
    );
  }
}

module.exports = { ChatBusiness };
