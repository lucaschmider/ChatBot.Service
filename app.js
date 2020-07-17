const chalk = require("chalk");
const express = require("express");
const mongoose = require("mongoose");
const { Controllers } = require("./Controllers");
const admin = require("firebase-admin");
const { ConfigService } = require("./Services/ConfigService");
const app = express();
const cors = require("cors");
const bodyParser = require("body-parser");
const { InfluxService } = require("./Services/InfluxService");

async function startup(configuration) {
  console.log(chalk.yellow(`Starting application for environment ${configuration.environment}`));

  try {
    console.log(chalk.yellow("Connecting to MongoDB"));
    await mongoose.connect(configuration.databaseUrl, {
      useNewUrlParser: true,
      useUnifiedTopology: true,
      useCreateIndex: true
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

  try {
    console.log(chalk.yellow("Connecting to InfluxDb"));
    await InfluxService.CreateConnection(ConfigService.loadedConfiguration.influx);
    console.log(chalk.green("Successfully connected to InfluxDb"));
  } catch (error) {
    console.error(chalk.red("Error occured while connecting to influx db:\n", error));
    return;
  }

  try {
    console.log(chalk.yellow("Configuring routers and middleware"));
    app.use(cors());
    app.use(bodyParser.json());
    app.use("/", Controllers.getRouter());

    app.listen(configuration.applicationPort, () => {
      console.log(chalk.bgGreen.black("Starup completed."));
      console.log(`Listening for requests at *:${configuration.applicationPort}`);
    });
  } catch (error) {
    console.error(chalk.red("Error occured while configuring routers and middleware:\n"), error);
  }
}

ConfigService.Create(process.argv[2])
  .then((message) => {
    console.log(chalk.green(message));
    startup(ConfigService.loadedConfiguration);
  })
  .catch((error) => {
    console.log(chalk.red(error));
  });
