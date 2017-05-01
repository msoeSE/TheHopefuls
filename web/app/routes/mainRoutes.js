var express = require("express");
var router = express.Router();
var mongoose = require("mongoose");
var config = require("../../config.json");
var request = require("request");

// For listing of supported status codes in this package
// https://www.npmjs.com/package/http-status-codes
var statusCodes = require("http-status-codes");


var userCtrl = require("../controllers/userCtrl");
var drivingSchoolCtrl = require("../controllers/drivingSchoolCtrl");
var drivingSessionCtrl = require("../controllers/drivingSessionCtrl");
var stateRegsCtrl = require("../controllers/stateRegsCtrl");
var linkSchoolCtrl = require("../controllers/linkSchoolCtrl");
var drivingDataCtrl = require("../controllers/drivingDataCtrl");

router.all("*", function(req, res, next){
	if(req.isAuthenticated()) {
		next();
	} else {
		res.status(statusCodes.UNAUTHORIZED);
		res.json({error: "Unauthorized"});
	}
});

// Get the JSON for the student with the specified _id
router.get("/students/:userId", function(req, res) {
	userCtrl.getStudent(req.params.userId, (user) => {
		res.json(user);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// Get all students
// TODO change to use driving school id
router.get("/allStudents", function(req, res) {
	userCtrl.getAllUsers((users)=>{
		res.json(users);
	}, (err)=>{
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// Get Instructors by school code
// TODO change to use drive school id
router.get("/instructors", function(req, res) {
	userCtrl.getAllUsers((users)=>{
		res.json(users);
	}, (err)=>{
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// Create a new student, return the JSON representation for the new student
router.post("/students", function(req, res) {
	userCtrl.createStudent(req.body, (student) => {
		res.status(statusCodes.CREATED);
		res.json(student);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// Get all existing driving sessions for a student
router.get("/students/:userId/drivingsessions", function(req, res) {
	drivingSessionCtrl.listDrivingSessions(req.params.userId, (drivingSessions) => {
		res.json(drivingSessions);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// Allow a new driving session to be added, return the JSON of the session
router.post("/students/:userId/drivingsessions", function(req, res) {
	drivingSessionCtrl.createDrivingSessions(req.params.userId, req.body, (results) => {
		res.status(statusCodes.CREATED);
		res.json(results);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// TODO: add later
// Get all existing driving schools in the system
router.get("/drivingschools", function(req, res) {
	res.status(statusCodes.NOT_IMPLEMENTED);
	res.json({});
});

// Create a new driving school to be added, return the school added
router.post("/drivingschools", function(req, res) {
	drivingSchoolCtrl.createSchool(req.body, (newSchool) => {
		res.status(statusCodes.CREATED);
		res.json(newSchool);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

router.get("/drivingschools/:schoolId", function(req, res) {
	drivingSchoolCtrl.getSchool(req.params.schoolId, (school) => {
		res.json(school);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// TODO: add later
router.get("/drivingschools/:schoolId/students", function(req, res) {
	res.status(statusCodes.NOT_IMPLEMENTED);
	res.json({});
});

// TODO: add later
// Remove a student from a driving school
router.delete("/drivingschools/:schoolId/students/:userId", function(req, res) {
	res.status(statusCodes.NOT_IMPLEMENTED);
	res.json({});
});

// TODO: add later
router.get("/drivingschools/:schoolId/instructors", function(req, res) {
	res.status(statusCodes.NOT_IMPLEMENTED);
	res.json({});
});

// TODO: add later
// allow a new instructor to be added, return the data added
router.post("/drivingschools/:schoolId/instructors", function(req, res) {
	res.status(statusCodes.NOT_IMPLEMENTED);
	res.json({});
});

// TODO: add later
// Remove an instructor from a driving school
router.delete("/drivingschools/:schoolId/instructors/:userId", function(req, res) {
	res.status(statusCodes.NOT_IMPLEMENTED);
	res.json({});
});

// GET the driving regulations for a specified state
router.get("/stateregulations/:state", function(req, res) {
	stateRegsCtrl.getStateRegs(req.params.state, (stateRegs) => {
		res.json(stateRegs);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// POST to link an account to a driving school
router.post("/linkacctoschool", function(req, res) {
	linkSchoolCtrl.linkAccToSchool(req.user.id, req.body.schoolId, (results) => {
		res.status(statusCodes.OK);
		res.json(results);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// GET current aggregate driving data (as a student)
router.get("/totalDrivingData", function(req, res) {
	drivingDataCtrl.getStudentData(req.user.id, (results) => {
		res.status(statusCodes.OK);
		res.json(results);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// POST current aggregate driving data (as instructor)
router.post("/totalDrivingData", function(req, res) {
	drivingDataCtrl.getStudentData(req.body.userId, (results) => {
		res.status(statusCodes.OK);
		res.json(results);
	}, (err) => {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

router.get("/weather/:lat/:long", function(req, res) {
	request({
		url: "https://api.darksky.net/forecast/" + config.DarkSykApiKey.Secret + "/" + req.params.lat + "," + req.params.long,
		json: true
	},
	function(error, response, body){
		if(!error && response.statusCode === 200){ //eslint-disable-line
			var weather = {
				summary: body.currently.summary,
				temperature: body.currently.temperature,
				icon: body.currently.icon
			};
			res.json(weather);
		} else {
			res.status(statusCodes.INTERNAL_SERVER_ERROR);
			res.json(error);
		}
	});
});

router.all("*", function(req, res){
	res.status(statusCodes.NOT_FOUND);
	res.json({error: "Not Found"});
});

module.exports = router;
