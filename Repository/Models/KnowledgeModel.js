const mongoose = require("mongoose");

const knowledgeSchema = mongoose.Schema({
  definitionType: {
    type: String,
    required: true
  },
  keyword: {
    type: String,
    required: true
  },
  description: {
    type: String,
    required: true
  }
});

var Knowledge = (module.exports = mongoose.model("knowledge", knowledgeSchema, "knowledge"));

module.exports.get = function (callback, limit) {
  Knowledge.find(callback).limit(limit);
};
