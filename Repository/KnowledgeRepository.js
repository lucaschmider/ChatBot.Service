const KnowledgeModel = require("./Models/KnowledgeModel");

class KnowledgeRepository {
  /**
   * Returns the definition of a given keyword with the corresponding definition
   * @param {string} keyword
   * @param {string} definitionType
   * @returns {Promise<string>}
   */
  static async GetDefinitionByKeywordAndDefinitionTypeAsync(keyword, definitionType) {
    const document = await KnowledgeModel.findOne({ keyword, definitionType }).exec();
    return document.toObject().description;
  }
}

module.exports = { KnowledgeRepository };
