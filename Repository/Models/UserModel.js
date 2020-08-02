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
    defaut: false
  },
  name: {
    type: String,
    required: false
  },
  department: {
    type: String,
    required: true
  },
  email: {
    type: String,
    required: true
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
