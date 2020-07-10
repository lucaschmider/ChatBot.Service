class StatisticsRepository {
  /**
   * Returns dummy data
   * @returns {Object} DummyData
   */
  static async GetStatisticsAsync() {
    return {
      Dummy: "Data"
    };
  }
}
module.exports = { StatisticsRepository };
