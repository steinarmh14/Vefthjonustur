const express = require("express");
const mongoose = require("mongoose");
const app = express();
const api = require("./api");
const port = 5000;

app.use(api);

mongoose.connect("localhost/producer");
mongoose.connection.once("open", () => {
  console.log("Connected to database");
  app.listen(port, function () {
    console.log("Web server started on port " + port);
  });
});