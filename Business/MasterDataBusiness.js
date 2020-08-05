const { MasterDataRepository } = require("../Repository/MasterDataRepository");
const { DialogFlowService } = require("../Services/DialogFlowService");

class MasterDataBusiness {
  static async GetKnowledgebaseAsync() {
    await DialogFlowService.GetEntities();
    const knowledgebase = await MasterDataRepository.GetAllData("knowledge");
  }
}

module.exports = { MasterDataBusiness };
