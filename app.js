const chalk = require("chalk");
const express = require("express");
const mongoose = require("mongoose");
const controllers = require("./Controllers");
const loadConfiguration = require("./ConfigService");
const app = express();

async function startup() {
  console.log(chalk.yellow(`Starting application for environment ${process.chatBot.environment}`));
  console.log(chalk.yellow("Connecting to MongoDB"));

  try {
    await mongoose.connect(process.chatBot.databaseUrl, {
      useNewUrlParser: true,
      useUnifiedTopology: true
    });
    console.log(chalk.green("Successfully connected to MongoDB."));
  } catch (error) {
    console.error(chalk.red("Error occured while connecting to MongoDB:\n"), error);
    return;
  }

  app.use("/", controllers);

  app.listen(process.chatBot.applicationPort, () =>
    console.log(`Listening for requests at *:${process.chatBot.applicationPort}`)
  );
}

loadConfiguration(process.argv[2])
  .then((configuration) => {
    process.chatBot = configuration;
    startup();
  })
  .catch((error) => {
    console.log(chalk.red(error));
  });
