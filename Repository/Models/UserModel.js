const mongoose = require("mongoose");

const userSchema = mongoose.Schema({
  uid: {
    type: String,
    required: true,
    unique: true
  },
  isAdmin: {
    type: Boolean,
    required: true,
    defaut: Date.now
  },
  name: {
    type: String,
    required: false
  },
  create_date: {
    type: Date,
    default: Date.now
  }
});

var User = (module.exports = mongoose.model("user", userSchema));

module.exports.get = function (callback, limit) {
  User.find(callback).limit(limit);
};
