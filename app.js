const chalk = require("chalk");
const express = require("express");
const mongoose = require("mongoose");
const controllers = require("./Controllers");
const app = express();

async function startup() {
  console.log(chalk.yellow("Connecting to MongoDB"));

  try {
    await mongoose.connect("mongodb://localhost/chatbot", {
      useNewUrlParser: true,
      useUnifiedTopology: true
    });
    console.log(chalk.green("Successfully connected to MongoDB."));
  } catch (error) {
    console.error(chalk.red("Error occured while connecting to MongoDB:\n"), error);
    return;
  }

  app.use("/", controllers);

  app.listen(3000, () => console.log("Listening"));
}

startup();
