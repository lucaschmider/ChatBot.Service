const { MasterDataRepository } = require("../Repository/MasterDataRepository");
const { DialogFlowService } = require("../Services/DialogFlowService");

class MasterDataBusiness {
  static async GetKeywordsAsync() {
    const knowledgebase = await MasterDataRepository.GetAllData("knowledge");
    const keywords = await DialogFlowService.GetKeywordsAsync();

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
}

module.exports = { MasterDataBusiness };
