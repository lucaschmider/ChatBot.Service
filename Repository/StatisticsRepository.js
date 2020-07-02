const TestModel = require("./Models/TestModel");

module.exports = {
  GetStatisticsAsync: async () => {
    const doc = await TestModel.create({
      name: "Max Mustertest",
      email: "m.mustertest@gmail.com",
    });

    return doc.toObject();
  },
};
