const fs = require("fs");
const path = require("path");

class ConfigService {
  static loadedConfiguration;

  static Create(environmentName) {
    return new Promise((resolve, reject) => {
      const exampleConfigPath = path.join(__dirname, "..", "env_example.json");
      if (!fs.existsSync(exampleConfigPath)) {
        reject(`Could not locate example configuration at ${exampleConfigPath}.`);
      }
      const exampleConfigurationString = fs.readFileSync(exampleConfigPath);
      const exampleConfiguration = JSON.parse(exampleConfigurationString);

      let configurationString;
      if (process.env.USE_CONFIG_FILE) {
        if (!fs.existsSync(process.env.USE_CONFIG_FILE)) {
          reject(
            `Application was instructed to use the configuration file ${process.env.USE_CONFIG_FILE}. But the file was not found.`
          );
        }
        configurationString = fs.readFileSync(process.env.USE_CONFIG_FILE);
      } else {
        const configPath = path.join(__dirname, "..", `env_${environmentName.toLowerCase()}.json`);
        if (!fs.existsSync(configPath)) {
          reject(`Could not locate environment configuration at ${configPath}.`);
        }
        configurationString = fs.readFileSync(configPath);
      }

      const environmentConfiguration = JSON.parse(configurationString);

      const missingKeys = [];
      Object.keys(exampleConfiguration).forEach((requiredKey) => {
        if (environmentConfiguration[requiredKey] === undefined || environmentConfiguration[requiredKey] === null) {
          missingKeys.push(requiredKey);
        }
      });

      if (missingKeys.length > 0) {
        reject(`Environment ${environmentName} is missing required properties: ${JSON.stringify(missingKeys)}`);
      }

      this.loadedConfiguration = environmentConfiguration;
      resolve(`Environment ${environmentName} loaded successfully.`);
    });
  }
}

module.exports = { ConfigService };
