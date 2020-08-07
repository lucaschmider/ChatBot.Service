const { MasterDataRepository } = require("../Repository/MasterDataRepository");
const { DialogFlowService } = require("../Services/DialogFlowService");
const { CreateKnowledgeResult } = require("./Models/CreateKnowledgeResult");

class MasterDataBusiness {
  static async GetKeywordsAsync() {
    const dialogFlowService = await DialogFlowService.getInstance();
    const knowledgebase = await MasterDataRepository.GetAllData("knowledge");
    const keywords = await dialogFlowService.GetKeywordsAsync();

    const result = knowledgebase.map((knownWord) => {
      const mappedKeyword = keywords.find((keyword) => keyword.value == knownWord.keyword);
      return {
        name: knownWord.keyword,
        keywords: [knownWord.keyword, ...mappedKeyword.synonyms],
        definitiontype: knownWord.definitionType,
        description: knownWord.description
      };
    });
    return result;
  }

  /**
   *
   * @param {object} knowledge
   * @param {string} knowledge.name
   * @param {string[]} knowledge.synonyms
   * @param {string} knowledge.definitiontype
   * @param {string} knowledge.description
   * @returns {Promise<CreateKnowledgeResult>}
   */
  static async CreateKnowledgeAsync(knowledge) {
    if (
      typeof knowledge.name !== "string" ||
      typeof knowledge.definitiontype !== "string" ||
      typeof knowledge.description !== "string" ||
      !(knowledge.synonyms instanceof Array) ||
      knowledge.synonyms.find((synonym) => typeof synonym !== "string")
    ) {
      return CreateKnowledgeResult.CreateForError("The provided object was malformed", "validation/malformed");
    }

    const dialogFlowService = await DialogFlowService.getInstance();
    const validDefinitionTypes = (await dialogFlowService.GetDefinitionTypesAsync()).map((type) => type.value);
    if (!validDefinitionTypes.includes(knowledge.definitiontype)) {
      return CreateKnowledgeResult.CreateForError(
        "Specified Definitiontype does not exist",
        "validation/invalid-definition-type"
      );
    }

    const databaseObject = {
      definitionType: knowledge.definitiontype,
      keyword: knowledge.name,
      description: knowledge.description
    };

    const dialogFlowObject = {
      value: knowledge.name,
      synonyms: knowledge.synonyms
    };

    await Promise.all([
      MasterDataRepository.InsertCollectionData("knowledge", databaseObject),
      dialogFlowService.CreateKnowledge(dialogFlowObject)
    ]);

    return CreateKnowledgeResult.CreateForSuccess();
  }
}

module.exports = { MasterDataBusiness };
