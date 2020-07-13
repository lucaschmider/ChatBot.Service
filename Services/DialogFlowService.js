const dialogflow = require("@google-cloud/dialogflow");
const { ConfigService } = require("./ConfigService");

class DialogFlowService {
  static async HandleMessage(chatId, message) {
    const responses = await DialogFlowService.SendRequest(chatId, message);
    const result = responses[0].queryResult;

    const parameters = {};
    for (const [key, value] of Object.entries(result.parameters.fields)) {
      parameters[key] = value.stringValue;
    }

    return {
      isCompleted: result.allRequiredParamsPresent,
      answerText: result.fulfillmentText,
      parameters
    };
  }

  static async SendRequest(chatId, message) {
    const configuration = ConfigService.loadedConfiguration.dialogflow;
    // Create a new session
    const sessionClient = new dialogflow.SessionsClient({
      credentials: configuration
    });
    const sessionPath = sessionClient.projectAgentSessionPath(configuration.project_id, chatId);

    // The text query request.
    const request = {
      session: sessionPath,
      queryInput: {
        text: {
          // The query to send to the dialogflow agent
          text: message,
          // The language used by the client (en-US)
          languageCode: "de-DE"
        }
      }
    };

    // Send request and log result
    const responses = await sessionClient.detectIntent(request);
    return responses;
  }
}

module.exports = { DialogFlowService };
