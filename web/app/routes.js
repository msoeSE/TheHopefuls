var mongoose = require("mongoose");
mongoose.connect("mongodb://localhost/appdb");

var User = require("./models/User.js");
var DrivingSchool = require("./models/DrivingSchool.js");

module.exports = function(app) {

	// server routes ===========================================================
	// handle things like api calls
	// authentication routes

	// TODO: add handling for bad api calls (i.e. invalid params, bad data formats)

	app.get("/api/students/:userId", function(req, res) {
		User.find({ userId: req.params.userId }, function(err, doc) {
			if(!err) {
				res.statusCode = 200;
				res.json(doc);
			} else {
				res.send(err);
			}
		});
	});

	// allow a user to be added, return the id for the user
	app.post("/api/students/", function(req, res) {
		var student = {
			userId: req.body.userId,
			email: req.body.email,
			firstName: req.body.firstName,
			lastName: req.body.lastName,
			password: req.body.password,
			userType: req.body.userType
			// state: req.body.state
		};

		var data = new User(student);
		data.save(function(err) {
			if(!err) {
				res.statusCode = 201;
				res.json(data);
			} else {
				res.send(err);
			}
		});
	});

	app.get("/api/students/:userId/drivingsessions", function(req, res) {
		res.statusCode = 200;
		res.json({});
	});

	// allow a new driving session to be added, return the data added
	app.post("/api/students/:userId/drivingsessions", function(req, res) {
		res.statusCode = 201;
		res.json({});
	});

	app.get("/api/drivingschools", function(req, res) {
		res.statusCode = 200;
		res.json({});
	});

	// allow a new driving school to be added, return the data added
	app.post("/api/drivingschools", function(req, res) {
		res.statusCode = 201;
		res.json({});
	});

	app.get("/api/drivingschools/:schoolId", function(req, res) {
		res.statusCode = 200;
		res.json({});
	});

	app.get("/api/drivingschools/:schoolId/students", function(req, res) {
		res.statusCode = 200;
		res.json({});
	});

	app.delete("/api/drivingschools/:schoolId/students/:userId", function(req, res) {
		//res.statusCode = 200;
		res.json({});
	});

	app.get("/api/drivingschools/:schoolId/instructors", function(req, res) {
		res.statusCode = 200;
		res.json({});
	});

	// allow a new instructor to be added, return the data added
	app.post("/api/drivingschools/:schoolId/instructors", function(req, res) {
		res.statusCode = 201;
		res.json({});
	});

	app.delete("/api/drivingschools/:schoolId/instructors/:userId", function(req, res) {
		//res.statusCode = 201;
		res.json({});
	});
	// frontend routes =========================================================
	// route to handle all angular requests
	app.get('*', function(req, res) {
		res.sendfile('./public/index.html');
	});

};
