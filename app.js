const chalk = require("chalk");
const express = require("express");
const mongoose = require("mongoose");
const controllers = require("./Controllers");
const admin = require("firebase-admin");
const { ConfigService } = require("./ConfigService");
const app = express();

async function startup(configuration) {
  console.log(chalk.yellow(`Starting application for environment ${configuration.environment}`));

  try {
    console.log(chalk.yellow("Connecting to MongoDB"));
    await mongoose.connect(configuration.databaseUrl, {
      useNewUrlParser: true,
      useUnifiedTopology: true
    });
    console.log(chalk.green("Successfully connected to MongoDB."));
  } catch (error) {
    console.error(chalk.red("Error occured while connecting to MongoDB:\n"), error);
    return;
  }

  try {
    console.log(chalk.yellow("Initializing Firebase"));
    admin.initializeApp({
      credential: admin.credential.cert(ConfigService.loadedConfiguration.firebase)
    });
    console.log(chalk.green("Successfully initialized Firebase"));
  } catch (error) {
    console.error(chalk.red("Error occured while initializing Firebase:\n", error));
    return;
  }

  app.use("/", controllers);

  app.listen(configuration.applicationPort, () =>
    console.log(`Listening for requests at *:${configuration.applicationPort}`)
  );
}

ConfigService.Create(process.argv[2])
  .then((message) => {
    console.log(chalk.green(message));
    startup(ConfigService.loadedConfiguration);
  })
  .catch((error) => {
    console.log(chalk.red(error));
  });
