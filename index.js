const express = require("express");
const app = express();
const mongoose = require("mongoose");

mongoose.connect("mongodb://localhost/chatbot", {
  useNewUrlParser: true,
  useUnifiedTopology: true
});
var db = mongoose.connection;

if (!db) console.log("Error connecting db");
else console.log("Db connected successfully");

const statisticsController = require("./Controllers/statistics");
app.use("/statistics", statisticsController);

app.listen(3000, () => console.log("Listening"));
