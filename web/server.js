// modules =================================================
var express        = require("express");
var app            = express();
var mongoose       = require("mongoose");
var bodyParser     = require("body-parser");
var methodOverride = require("method-override");

// configuration ===========================================

// config files
var db = require("./config/db");
var prodPort = 80;
var devPort = 8080;
// mongoose.connect(db.url); // connect to our mongoDB database (commented out after you enter in your own credentials)

// get all data/stuff of the body (POST) parameters
app.use(bodyParser.json()); // parse application/json
app.use(bodyParser.json({ type: "application/vnd.api+json" }));
app.use(bodyParser.urlencoded({ extended: true }));

app.use(methodOverride("X-HTTP-Method-Override"));
if(process.env.NODE_ENV === "production") {
	app.use(express.static(__dirname + "/dist"));
	var port = process.env.PORT || prodPort;
} else {
	app.use(express.static(__dirname + "/public"));
	var port = process.env.PORT || devPort;
}
app.use("/libs", express.static(__dirname + "/libs"));

// routes ==================================================
// require("./app/routes")(app); // pass our application into our routes
var routes = require("./app/routes/mainRoutes");
app.use("/api", routes);

var index = require("./app/routes/index");
app.use("/", index);

// start app ===============================================
app.listen(port);
console.log("Magic happens on port " + port); // eslint-disable-line
exports = module.exports = app;
