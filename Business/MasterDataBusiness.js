const { MasterDataRepository } = require("../Repository/MasterDataRepository");

export class MasterDataBusiness {
  static async GetKnowledgebaseAsync() {
    const knowledgebase = await MasterDataRepository.GetAllData("knowledge");
    console.log(knowledgebase);
  }
}

module.exports = { MasterDataBusiness };
