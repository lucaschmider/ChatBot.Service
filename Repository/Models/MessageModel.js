const mongoose = require("mongoose");

const messageSchema = mongoose.Schema({
  receipient: {
    type: String,
    required: true
  },
  message: {
    type: String,
    required: true
  },
  conversationFinished: {
    type: Boolean,
    required: true,
    default: false
  },
  create_date: {
    type: Date,
    default: Date.now
  }
});

var Message = (module.exports = mongoose.model("message", messageSchema));

module.exports.get = function (callback, limit) {
  Message.find(callback).limit(limit);
};
