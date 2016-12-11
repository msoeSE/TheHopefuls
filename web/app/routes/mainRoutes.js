var express = require("express");
var router = express.Router();
var mongoose = require("mongoose");
mongoose.connect("mongodb://localhost/routerdb");

var User = require("../models/User");
var DrivingSchool = require("../models/DrivingSchool");
var userCtrl = require("../controllers/userCtrl");
var drivingSchoolCtrl = require("../controllers/drivingSchoolCtrl");


// server routes ===========================================================
// handle things like api calls
// authentication routes

// Get the JSON for the student with the specified _id
router.get("/students/:_id", function(req, res) {
  userCtrl.getStudent(req, res);
});

// Create a new student, return the JSON represntation for the new student
router.post("/students/", function(req, res) {
  userCtrl.createStudent(req, res);
});

// Get all existing driving sessions for a student
router.get("/students/:userId/drivingsessions", function(req, res) {
	res.statusCode = 200;
	res.json({});
});

// allow a new driving session to be added, return the data added
router.post("/students/:userId/drivingsessions", function(req, res) {
	res.statusCode = 201;
	res.json({});
});

//TODO: add later
// Get all existing driving schools in the system
router.get("/drivingschools", function(req, res) {
	res.statusCode = 200;
	res.json({});
});

// Create a new driving school to be added, return the school added
router.post("/drivingschools", function(req, res) {
  drivingSchoolCtrl.createSchool(req, res);
});

router.get("/drivingschools/:schoolId", function(req, res) {
  drivingSchoolCtrl.getSchool(req, res);
});

//TODO: add later
router.get("/drivingschools/:schoolId/students", function(req, res) {
	res.statusCode = 200;
	res.json({});
});

//TODO: add later
// Remove a student from a driving school
router.delete("/drivingschools/:schoolId/students/:userId", function(req, res) {
	//res.statusCode = 200;
	res.json({});
});

//TODO: add later
router.get("/drivingschools/:schoolId/instructors", function(req, res) {
	res.statusCode = 200;
	res.json({});
});

//TODO: add later
// allow a new instructor to be added, return the data added
router.post("/drivingschools/:schoolId/instructors", function(req, res) {
	res.statusCode = 201;
	res.json({});
});

//TODO: add later
// Remove an instructor from a driving school
router.delete("/drivingschools/:schoolId/instructors/:userId", function(req, res) {
	//res.statusCode = 201;
	res.json({});
});

module.exports = router;
