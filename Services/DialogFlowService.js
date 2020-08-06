const dialogflow = require("@google-cloud/dialogflow");
const { ConfigService } = require("./ConfigService");
const { Compute, JWT, UserRefreshClient, GoogleAuth } = require("google-auth-library");

class DialogFlowService {
  /**
   * @type {DialogFlowService}
   */
  static #instance;

  /**
   * @type {string}
   */
  static #dialogFlowBaseUrl = "https://dialogflow.googleapis.com/v2";

  /**
   * @type {Compute | JWT | UserRefreshClient}
   */
  #client;

  /**
   * Returns the saved ServiceInstance
   */
  static async getInstance() {
    if (!DialogFlowService.#instance) {
      const auth = new GoogleAuth({
        credentials: {
          private_key: ConfigService.loadedConfiguration.dialogflow.private_key,
          client_email: ConfigService.loadedConfiguration.dialogflow.client_email
        },
        scopes: ["https://www.googleapis.com/auth/cloud-platform", "https://www.googleapis.com/auth/dialogflow"]
      });
      const client = await auth.getClient();
      DialogFlowService.#instance = new DialogFlowService(client);
    }

    return DialogFlowService.#instance;
  }

  /**
   * Creates a new instance of the Service.
   * @private Use getInstance() instead
   * @param {Compute | JWT | UserRefreshClient} client
   */
  constructor(client) {
    this.#client = client;
  }

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
  GetDefinitionTypesAsync() {
    return this.GetEntitiesAsync("projects/hidden-howl-282919/agent/entityTypes/2bf0d912-ae7a-4bd1-a929-448384dc1ba6");
  }

  /**
   * Returns all Keywords alongside with their synonyms
   */
  GetKeywordsAsync() {
    return this.GetEntitiesAsync("projects/hidden-howl-282919/agent/entityTypes/0f587498-d18f-429a-bf4c-88c5fb9f5c63");
  }

  /**
   * Returns all entites defined in the entity type name
   * @private
   * @param {string} entityTypeName
   */
  async GetEntitiesAsync(entityTypeName) {
    const url = `${DialogFlowService.#dialogFlowBaseUrl}/${entityTypeName}`;
    const response = await this.#client.request({ url });
    return response.data.entities;
  }

  /**
   * Creates a new knowledge
   * @param {object} entity
   * @param {string} entity.value
   * @param {string[]} entity.synonyms
   */
  async CreateKnowledge(entity) {
    const keywordPath = "projects/hidden-howl-282919/agent/entityTypes/0f587498-d18f-429a-bf4c-88c5fb9f5c63";
    const url = `${DialogFlowService.#dialogFlowBaseUrl}/${keywordPath}/entities:batchCreate`;
    this.#client.request({
      url,
      data: {
        entities: [entity]
      },
      method: "POST"
    });
  }

  /**
   * Deletes the specified knowledge
   * @param {string} term
   */
  async deleteKnowledge(term) {
    const keywordPath = "projects/hidden-howl-282919/agent/entityTypes/0f587498-d18f-429a-bf4c-88c5fb9f5c63";
    const url = `${DialogFlowService.#dialogFlowBaseUrl}/${keywordPath}/entities:batchDelete`;
    this.#client.request({
      url,
      data: {
        entities: [term]
      },
      method: "POST"
    });
  }
}

module.exports = { DialogFlowService };
