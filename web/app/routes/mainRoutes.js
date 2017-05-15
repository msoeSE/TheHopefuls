var express = require("express");
var router = express.Router();
var mongoose = require("mongoose");
var config = require("../../config.json");
var request = require("request");
var moment = require("moment"); // eslint-disable-line

// For listing of supported status codes in this package
// https://www.npmjs.com/package/http-status-codes
var statusCodes = require("http-status-codes");

var userCtrl = require("../controllers/userCtrl");
var drivingSchoolCtrl = require("../controllers/drivingSchoolCtrl");
var drivingSessionCtrl = require("../controllers/drivingSessionCtrl");
var stateRegsCtrl = require("../controllers/stateRegsCtrl");
var linkSchoolCtrl = require("../controllers/linkSchoolCtrl");
var drivingDataCtrl = require("../controllers/drivingDataCtrl");

/* eslint-disable */
if(config.LogAPIRequestsToConsole){
	console.log("All api Calls will be logged to console. Brace yourself...");
	console.log("To disable, turn off the logging option in the config file");
	router.all("*", function(req, res, next){
		console.log(`Request Received - ${moment().format("MMMM Do YYYY, h:mm:ss a")}`);
		console.log(`Method: ${req.method} - Endpoint: ${req.originalUrl}`);
		console.log(`Headers: ${JSON.stringify(req.headers, null, 4)}`);
		if(req.body){
			if(typeof(req.body) === "object"){
				console.log(`Body: ${JSON.stringify(req.body, null, 4)}`);
			} else {
				console.log(`Body: ${req.body}`);
			}
		}
		if(req.isAuthenticated())
			console.log(`User: ${JSON.stringify(req.user, null, 4)}`);
		next();
	});
}
/* eslint-enable */


router.all("*", function(req, res, next){
	if(req.isAuthenticated()) {
		next();
	} else {
		res.status(statusCodes.UNAUTHORIZED);
		res.json({error: "You are not logged in"});
	}
});

router.all("*", function(req, res, next){
	userCtrl.getUser(req.user.id, function(doc){
		if(doc){
			req.user.userType = doc.userType;
			req.user.mongoID = doc._id;
			req.user.schoolId = doc.schoolId;
		}
		next();
	}, function (err){
		console.log(err); // eslint-disable-line
		res.redirect("/");
	});
});

/*	You can use this middleware to add a wrapper for json responses
	This was mainly needed for DataTables, but can be used for other uses
	Use like /api/endpointThatReturnsJSON?jsonwrapper=keyForObject
	and this would return {"keyForObject": originalObject}
*/
router.all("*", function(req, res, next){
	var wrapper = req.query.jsonwrapper;
	if(wrapper){
		res.jsonOriginal = res.json;
		res.json = obj => res.jsonOriginal({[wrapper]: obj});
	}
	next();
});

function isInstructorOf(instructor, student){
	if(instructor.userType === "student" || student.userType !== "student")
		return false;
	return instructor.schoolId === student.schoolId;
}

function canSeeStats(currentUser, student){
	return currentUser.userType === "admin"
		|| currentUser.id === student.userId
		|| isInstructorOf(currentUser, student);
}

function standardErrorHandler(res){
	var res = res;
	return (err)=>{
		if(config.LogAPIRequestsErrors)
			console.log(JSON.stringify(err, null, 4)); // eslint-disable-line
		res.status(statusCodes.BAD_REQUEST);
		res.json(err);
	};
}

// Get the JSON for the student with the specified _id
router.get("/students/:userId", function(req, res) {
	userCtrl.getStudent(req.params.userId, (user)=>{
		console.log(user);
		if(!canSeeStats(req.user, user)){
			res.status(statusCodes.UNAUTHORIZED);
			res.json({message: "You cannot see this person's driving stats"});
		} else {
			res.json(user);
		}
	}, standardErrorHandler(res));
});

// Get all students
// TODO change to use driving school id
router.get("/allStudents", function(req, res) {
	if(req.user.userType !== "admin"){
		res.status(statusCodes.GONE);
		res.json({message: "This endpoint is now deprecated, please use the " +
		"'/drivingschools/:schoolId/students" +
		"endpoint to get a specific school's students."});
		return;
	}
	userCtrl.getAllUsers((users)=>{
		res.json(users);
	}, standardErrorHandler(res));
});

// Get Instructors by school code
// TODO change to use drive school id
router.get("/instructors", function(req, res) {
	if(req.user.userType !== "admin"){
		res.status(statusCodes.GONE);
		res.json({message: "This endpoint is now deprecated, please use the " +
		"'/drivingschools/:schoolId/instructors'" +
		"endpoint to get a specific school's instructors."});
		return;
	}
	userCtrl.getAllUsers((users)=>{
		res.json(users);
	}, standardErrorHandler(res));
});

// Get all existing driving sessions for a student
router.get("/students/:userId/drivingsessions", function(req, res) {
	userCtrl.getUserByMongoId(req.params.userId, (user)=>{
		if(!canSeeStats(req.user, user)){
			res.status(statusCodes.UNAUTHORIZED);
			res.json({message: "You cannot see this person's driving stats"});
			return;
		}
		drivingSessionCtrl.listDrivingSessions(req.params.userId, (drivingSessions) => {
			res.json(drivingSessions);
		}, (err) => {
			res.status(statusCodes.BAD_REQUEST);
			res.json(err);
		});
	}, standardErrorHandler(res));
});

// Allow a new driving session to be added, return the JSON of the session
router.post("/students/:userId/drivingsessions", function(req, res) {
	userCtrl.getUserByMongoId(req.params.userId, (user)=>{
		if(!canSeeStats(req.user, user)){
			res.status(statusCodes.UNAUTHORIZED);
			res.json({message: "You cannot edit this person's driving stats"});
			return;
		}
		// Check to see if multiple sessions being adding
		var functionToCall = Array.isArray(req.body) ? // eslint-disable-line
			drivingSessionCtrl.createDrivingSessions :
			drivingSessionCtrl.createDrivingSession;

		functionToCall(req.params.userId, req.body, (results) => {
			res.status(statusCodes.CREATED);
			res.json(results);
		}, standardErrorHandler(res));
	}, standardErrorHandler(res));
});

// TODO: add later
// Get all existing driving schools in the system
router.get("/drivingschools", function(req, res) {
	res.status(statusCodes.NOT_IMPLEMENTED);
	res.json({});
});

// Create a new driving school to be added, return the school added
router.post("/drivingschools", function(req, res) {
	if(req.user.userType !== "admin"){
		res.status(statusCodes.UNAUTHORIZED);
		res.json({message: "You cannot create a driving school"});
		return;
	}
	drivingSchoolCtrl.createSchool(req.body, (newSchool) => {
		res.status(statusCodes.CREATED);
		res.json(newSchool);
	}, standardErrorHandler(res));
});

router.get("/drivingschools/:schoolId", function(req, res) {
	userCtrl.getUser(req.user.id, (user)=>{
		if(user.schoolId !== req.params.schoolId && user.userType !== "admin"){
			res.status(statusCodes.UNAUTHORIZED);
			res.json({message: "You cannot see this school's information"});
			return;
		}
		drivingSchoolCtrl.getSchool(req.params.schoolId, (school) => {
			res.json(school);
		}, standardErrorHandler(res));
	}, standardErrorHandler(res));
});

// TODO: add later
router.get("/drivingschools/:schoolId/students", function(req, res) {
	userCtrl.getUser(req.user.id, (user)=>{
		if(user.schoolId !== req.params.schoolId && user.userType === "student"){
			res.status(statusCodes.UNAUTHORIZED);
			res.json({message: "You cannot see this school's information"});
			return;
		}
		userCtrl.getAllStudentsForSchool(req.params.schoolId, students => {
			res.json(students);
		}, standardErrorHandler(res));
	});
});

// TODO: add later
function canRemoveStudentFromSchool(currentUser, school, student){
	if(currentUser.id === student.userId)
		return true;
	if(currentUser.userType === "admin")
		return true;
	if(currentUser.userType !== "student" && currentUser.schoolId === school.schoolId)
		return true;
	return false;
}

// Remove a student from a driving school
router.delete("/drivingschools/:schoolId/students/:userId", function(req, res) {
	var user;
	var school;
	new Promise(function(resolve, reject) { // Check user exists
		userCtrl.getUser(req.params.userId, (x)=>{
			user = x;
			resolve();
		}, reject);
	}).then(() => { // Check school exists
		return new Promise(function(resolve, reject) {
			drivingSchoolCtrl.getSchool(req.params.schoolId, (x)=>{
				school = x;
				resolve();
			}, reject);
		});
	}).then(() => { // Check current user can remove from school
		return new Promise(function(resolve, reject) {
			if(canRemoveStudentFromSchool(req.user, school, user)){
				resolve();
			}else{
				res.status(statusCodes.UNAUTHORIZED);
				res.json({"message": "Cannot remove this student from school"});
				reject();
			}
		});
	}).then(() => { // Do it
		user.schoolId = undefined;
		user.save((err)=>{
			if(err){
				reject(err);
				return;
			}
			drivingSchoolCtrl.removeStudentFromSchool(user, resolve, reject);
		});
	}).catch(err => {
		if(err)
			standardErrorHandler(res)(err);
	});
});

// TODO: add later
router.get("/drivingschools/:schoolId/instructors", function(req, res) {
	if((req.user.userType === "owner" && req.user.schoolId == req.params.schoolId) || req.user.userType === "admin"){ //eslint-disable-line
		userCtrl.getAllInstructorsForSchool(req.params.schoolId, instructors => {
			res.json(instructors);
		}, standardErrorHandler);
	} else {
		res.status(statusCodes.UNAUTHORIZED);
		res.json({"message": "You cannot see the instructors for this school"});
	}
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

router.get("/states", function(req, res){
	stateRegsCtrl.getStates((states)=>{
		res.json(states);
	}, standardErrorHandler(res));
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

// GET current aggregate driving data
router.get("/totalDrivingData/:userId", function(req, res) {
	drivingDataCtrl.getStudentData(req.params.userId, (results) => {
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
