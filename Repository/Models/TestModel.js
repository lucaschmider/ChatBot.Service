var mongoose = require("mongoose");

const testSchema = mongoose.Schema({
  name: {
    type: String,
    required: true,
  },
  email: {
    type: String,
    required: true,
  },
  gender: String,
  phone: String,
  create_date: {
    type: Date,
    default: Date.now,
  },
});

var Test = (module.exports = mongoose.model("test", testSchema));

module.exports.get = function (callback, limit) {
  Test.find(callback).limit(limit);
};
