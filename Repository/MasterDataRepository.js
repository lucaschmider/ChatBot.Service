const { connection } = require("mongoose");
const { ObjectId } = require("mongodb");
const { DialogFlowService } = require("../Services/DialogFlowService");
class MasterDataRepository {
  static #allowedCollections = ["departments", "knowledge"];
  /**
   * Returns all documents contained in the specified collection
   * @param {string} collection The name of the collection to load
   * @returns {Promise<any[]>}
   */
  static async GetAllData(collection) {
    if (!this.#allowedCollections.includes(collection)) {
      throw new Error(`Collection '${collection}' is not meant to be used with the MasterDataRepository.`);
    }
    return connection.db.collection(collection).find().toArray();
  }

  /**
   * Returns a list of all fields along with a display friendly name of the specified collection
   * @param {string} collection The name of the collection
   */
  static async GetCollectionScheme(collection) {
    switch (collection) {
      case "departments":
        return {
          collection: "departments",
          fields: [{ key: "departmentName", name: "Abteilung", type: "text" }]
        };
      case "knowledge":
        const dialogFlowService = await DialogFlowService.getInstance();
        const options = (await dialogFlowService.GetDefinitionTypesAsync()).map((type) => type.value);
        return {
          collection: "knowledge",
          fields: [
            { key: "name", name: "Titel", type: "text" },
            { key: "keywords", name: "Synonyme (Kommasepariert)", type: "text" },
            { key: "definitiontype", name: "Definition", type: "options", options },
            { key: "description", name: "Beschreibung", type: "text" }
          ]
        };
      default:
        throw new Error(`Collection '${collection}' is not meant to be used with the MasterDataRepository.`);
    }
  }

  /**
   * Inserts the specified data into the collection
   * @param {string} collection
   * @param {any} data
   */
  static async InsertCollectionData(collection, data) {
    if (!this.#allowedCollections.includes(collection)) {
      throw new Error(`Collection '${collection}' is not meant to be used with the MasterDataRepository.`);
    }

    return await connection.db.collection(collection).insertOne(data);
  }

  /**
   * Deletes all data matching the criteria
   * @param {string} collection
   * @param {any} criteria
   */
  static async DeleteData(collection, criteria) {
    if (!this.#allowedCollections.includes(collection)) {
      throw new Error(`Collection '${collection}' is not meant to be used with the MasterDataRepository.`);
    }
    return connection.db.collection(collection).deleteMany(criteria);
  }

  /**
   * Deletes the specified docId
   * @param {string} collection
   * @param {string} docId
   */
  static async DeleteDataById(collection, docId) {
    if (!this.#allowedCollections.includes(collection)) {
      throw new Error(`Collection '${collection}' is not meant to be used with the MasterDataRepository.`);
    }
    return connection.db.collection(collection).deleteOne({ _id: ObjectId(docId) });
  }
}

module.exports = { MasterDataRepository };
