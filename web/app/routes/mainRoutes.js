var express = require("express");
var router = express.Router();
var mongoose = require("mongoose");
mongoose.connect("mongodb://localhost/routerdb");

var User = require("../models/User");
var DrivingSchool = require("../models/DrivingSchool");
var userCtrl = require("../controllers/userCtrl");


// server routes ===========================================================
// handle things like api calls
// authentication routes

router.get("/students/:_id", function(req, res) {
  userCtrl.getStudent(req, res);
});

// allow a user to be added, return the id for the user
router.post("/students/", function(req, res) {
  userCtrl.createStudent(req, res);
});

router.get("/students/:userId/drivingsessions", function(req, res) {
	res.statusCode = 200;
	res.json({});
});

// allow a new driving session to be added, return the data added
router.post("/students/:userId/drivingsessions", function(req, res) {
	res.statusCode = 201;
	res.json({});
});

router.get("/drivingschools", function(req, res) {
	res.statusCode = 200;
	res.json({});
});

// allow a new driving school to be added, return the data added
router.post("/drivingschools", function(req, res) {
	res.statusCode = 201;
	res.json({});
});

router.get("/drivingschools/:schoolId", function(req, res) {
	res.statusCode = 200;
	res.json({});
});

router.get("/drivingschools/:schoolId/students", function(req, res) {
	res.statusCode = 200;
	res.json({});
});

router.delete("/drivingschools/:schoolId/students/:userId", function(req, res) {
	//res.statusCode = 200;
	res.json({});
});

router.get("/drivingschools/:schoolId/instructors", function(req, res) {
	res.statusCode = 200;
	res.json({});
});

// allow a new instructor to be added, return the data added
router.post("/drivingschools/:schoolId/instructors", function(req, res) {
	res.statusCode = 201;
	res.json({});
});

router.delete("/drivingschools/:schoolId/instructors/:userId", function(req, res) {
	//res.statusCode = 201;
	res.json({});
});


module.exports = router;
