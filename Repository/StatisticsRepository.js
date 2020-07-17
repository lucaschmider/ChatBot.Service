const { InfluxService } = require("../Services/InfluxService");
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

  static async RegisterFeedback(rating, department) {
    await InfluxService.GetConnection().writePoints([
      {
        measurement: "user_satisfaction",
        tags: {
          department
        },
        fields: { rating }
      }
    ]);
  }
}
module.exports = { StatisticsRepository };
