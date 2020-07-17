const Influx = require("influx");

class InfluxService {
  static __connection;
  static async CreateConnection({ host, database }) {
    InfluxService.__connection = new Influx.InfluxDB({
      host,
      database,
      schema: [
        {
          measurement: "user_satisfaction",
          fields: {
            rating: Influx.FieldType.INTEGER
          },
          tags: ["department"]
        }
      ]
    });
  }

  /**
   * @returns {Influx.InfluxDB} Returns a influx db connection.
   */
  static GetConnection() {
    return this.__connection;
  }
}

module.exports = { InfluxService };
