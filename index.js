const express = require("express");
const app = express();

const statisticsController = require("./Controllers/statistics");

app.use("/statistics", statisticsController);

app.listen(3000, () => console.log("Listening"));
