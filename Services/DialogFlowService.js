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
      detectedIntent: result.intent.name,
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

  /**
   * Returns all DefinitionTypes alongside with their synonyms
   */
  static GetDefinitionTypesAsync() {
    return DialogFlowService.GetEntities(
      "projects/hidden-howl-282919/agent/entityTypes/2bf0d912-ae7a-4bd1-a929-448384dc1ba6"
    );
  }

  /**
   * Returns all Keywords alongside with their synonyms
   */
  static GetKeywordsAsync() {
    return DialogFlowService.GetEntities(
      "projects/hidden-howl-282919/agent/entityTypes/0f587498-d18f-429a-bf4c-88c5fb9f5c63"
    );
  }

  /**
   * Returns all entites defined in the entity type name
   * @private
   * @param {string} entityTypeName
   */
  static async GetEntities(entityTypeName) {
    const configuration = ConfigService.loadedConfiguration.dialogflow;
    // Instantiates clients
    const entityTypesClient = new dialogflow.EntityTypesClient({
      credentials: configuration
    });

    // The path to the agent the entity types belong to.
    const agentPath = entityTypesClient.agentPath(configuration.project_id);

    const request = {
      parent: agentPath
    };

    // DefinitionType Path:
    // Call the client library to retrieve a list of all existing entity types.
    // const response = await entityTypesClient.listEntityTypes(request);

    const [response] = await entityTypesClient.getEntityType({
      name: ""
    });
    return response.entities;
  }
}

module.exports = { DialogFlowService };
