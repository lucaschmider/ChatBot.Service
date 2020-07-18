const { InfluxService } = require("../Services/InfluxService");
const { ConfigService } = require("../Services/ConfigService");

class StatisticsRepository {
  /**
   * Returns the average rating by department
   */
  static async GetStatisticsAsync() {
    const data = await InfluxService.GetConnection().query(
      `SELECT mean("rating") FROM "user_satisfaction" GROUP BY time(30m), "department" FILL(previous)`
    );
    return data.groups().map((group) => {
      return {
        department: group.tags.department,
        ratings: group.rows.map((slice) => {
          return { rating: slice.mean, time: slice.time };
        })
      };
    });
  }

  /**
   * Registers a feedback at the database
   * @param {number} rating, between 1 and 5
   * @param {*} department of the user
   */
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
