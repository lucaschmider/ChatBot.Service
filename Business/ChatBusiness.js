const { ChatRepository } = require("../Repository/ChatRepository");
const { DialogFlowService } = require("../Services/DialogFlowService");
const { ConfigService } = require("../Services/ConfigService");
const { KnowledgeRepository } = require("../Repository/KnowledgeRepository");
const chalk = require("chalk");

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
    try {
      const description = await KnowledgeRepository.GetDefinitionByKeywordAndDefinitionTypeAsync(
        parameters.keyword,
        parameters.definitiontype
      );
      await ChatRepository.CreateMessageAsync(userId, description, true);
    } catch (error) {
      console.log(
        chalk.red(`An unexpected error occured while answering a question (${JSON.stringify(parameters)})\n`, error)
      );
    }
  }
}

module.exports = { ChatBusiness };
