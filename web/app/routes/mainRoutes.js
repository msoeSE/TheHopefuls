var express = require("express");
var router = express.Router();
var mongoose = require("mongoose");

// For listing of supported status codes in this package
// https://www.npmjs.com/package/http-status-codes
var statusCodes = require("http-status-codes");

// TODO Replace with config
mongoose.connect("mongodb://localhost/routerdb");

var userCtrl = require("../controllers/userCtrl");
var drivingSchoolCtrl = require("../controllers/drivingSchoolCtrl");
var drivingSessionCtrl = require("../controllers/drivingSessionCtrl");
var stateRegsCtrl = require("../controllers/stateRegsCtrl");

function ensureAuthenticated(req, res, next){
	if(req.isAuthenticated())
		next();
	res.status(statusCodes.FORBIDDEN);
	res.json({error: "Not Authenticated"});
}

app.get("/profile", ensureAuthenticated,
	function(req, res){
		if(req.user)
			res.write(JSON.stringify(req.user)); // eslint-disable-line
		res.end();
	}
);

// Get the JSON for the student with the specified _id
router.get("/students/:userId", function(req, res) {
	userCtrl.getStudent(req.params.userId, (user)=>{
		res.json(user);
	}, (err)=>{
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// Create a new student, return the JSON representation for the new student
router.post("/students", function(req, res) {
	userCtrl.createStudent(req.body, (student)=>{
		res.status(statusCodes.CREATED);
		res.json(student);
	}, (err)=>{
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// Get all existing driving sessions for a student
router.get("/students/:userId/drivingsessions", function(req, res) {
	drivingSessionCtrl.listDrivingSessions(req.params.userId, (drivingSessions)=>{
		res.json(drivingSessions);
	},(err)=>{
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

// Allow a new driving session to be added, return the JSON of the session
router.post("/students/:userId/drivingsessions", function(req, res) {
	drivingSessionCtrl.createDrivingSessions(req.params.userId, req.body, (results)=>{
		res.status(statusCodes.CREATED);
		res.json(results);
	}, (err)=>{
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
	drivingSchoolCtrl.createSchool(req.body, (newSchool)=>{
		res.status(statusCodes.CREATED);
		res.json(newSchool);
	}, (err)=> {
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

router.get("/drivingschools/:schoolId", function(req, res) {
	drivingSchoolCtrl.getSchool(req.params.schoolId, (school)=>{
		res.json(school);
	}, (err)=>{
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
	stateRegsCtrl.getStateRegs(req.params.state, (stateRegs)=>{
		res.json(stateRegs);
	}, (err)=>{
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	});
});

module.exports = router;
