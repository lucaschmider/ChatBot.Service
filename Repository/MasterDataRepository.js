const { connection } = require("mongoose");

class MasterDataRepository {
  static #allowedCollections = ["departments", "knowledge"];
  static #collectionSchemes = [
    {
      collection: "departments",
      fields: [{ key: "departmentName", name: "Abteilung" }]
    },
    {
      collection: "knowledge",
      fields: [
        { key: "name", name: "Titel" },
        { key: "keywords", name: "Synonyme (Kommasepariert)" },
        { key: "definitiontype", name: "Definition" },
        { key: "description", name: "Beschreibung" }
      ]
    }
  ];
  /**
   * Returns all documents contained in the specified collection
   * @param {string} collection The name of the collection to load
   * @returns {Promise<any[]>}
   */
  static async GetAllData(collection) {
    if (!this.#allowedCollections.includes(collection)) {
      throw new Error(`Collection '${collection}' is not meant to be used with the MasterDataRepository.`);
    }

    switch (collection) {
      case "departments":
        return new Promise((resolve) =>
          resolve([
            { departmentName: "Human Resources", id: "0" },
            { departmentName: "External Sales", id: "1" },
            { departmentName: "IT-Department", id: "2" },
            { departmentName: "Key Accounts", id: "3" },
            { departmentName: "Manufacturing Radar", id: "4" },
            { departmentName: "Manufacturing Capacitive", id: "5" }
          ])
        );

      default:
        return connection.db.collection(collection).find().toArray();
    }
  }

  /**
   * Returns a list of all fields along with a display friendly name of the specified collection
   * @param {string} collection The name of the collection
   */
  static async GetCollectionScheme(collection) {
    if (!this.#allowedCollections.includes(collection)) {
      throw new Error(`Collection '${collection}' is not meant to be used with the MasterDataRepository.`);
    }
    return this.#collectionSchemes.find((x) => x.collection == collection);
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

    return connection.db.collection(collection).insertOne(data);
  }
}

module.exports = { MasterDataRepository };
