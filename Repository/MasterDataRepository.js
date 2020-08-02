class MasterDataRepository {
  static #allowedCollections = ["departments"];
  static async GetAllData(collection) {
    if (!this.#allowedCollections.includes(collection)) {
      throw new Error("This collection is not meant to be used with the MasterDataRepository.");
    }
    if (collection == "departments") {
      return new Promise((resolve) =>
        resolve([
          { departmentName: "Human Resources" },
          { departmentName: "External Sales" },
          { departmentName: "IT-Department" },
          { departmentName: "Key Accounts" },
          { departmentName: "Manufacturing Radar" },
          { departmentName: "Manufacturing Capacitive" }
        ])
      );
    }
  }
}

module.exports = { MasterDataRepository };
