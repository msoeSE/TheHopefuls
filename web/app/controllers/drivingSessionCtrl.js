var DrivingSession = require("../models/DrivingSession");
var User = require("../models/User");
var _ = require("underscore");
var geolib = require("geolib");
var of = require("../libs/objectFunctions");
var moment = require("moment"); // eslint-disable-line

function calculateDistance(drivePoints) {
	// drivePoints obj has lat, lon, speed, time
	var geolibPoints = drivePoints.map((point) => {
		return {latitude: point.lat, longitude: point.lon};
	});
	return geolibPoints.reduce((data, point) => {
		data.distance += geolib.getDistance(data.point, point);
		data.point = point;
		return data;
	}, {distance: 0, location: geolibPoints.shift()}).distance;
}

function calculateDuration(start, end) {
	return moment(end).diff(moment(start));
}

function updateUser(userId, session, callback, error){
	User.findOneAndUpdate({
		loginDetails: { userId: userId }
	}, {$push: {drivingSessions: session}},
	function(err, doc) {
		if (err) {
			error({
				"message": "Error updating user's driving session",
				"error": err
			});
			return;
		}
		callback(session);
	});
}

exports.createDrivingSession = function(userId, driveSession, callback, error) {
	var requiredItems = ["DrivePoints", "UnsyncDrive", "DriveWeatherData"];
	var missingItems = of.MissingProperties(driveSession, requiredItems);
	if(_.any(missingItems)){
		error({
			"message": "Required fields for creating driving session are missing",
			"missing-items": missingItems
		});
		return;
	}
	var startTime = new Date(driveSession.UnsyncDrive.startTime);
	var endTime = new Date(driveSession.UnsyncDrive.endTime);
	var distance = calculateDistance(driveSession.DrivePoints);
	var duration = calculateDuration(driveSession.UnsyncDrive.startTime,
		driveSession.UnsyncDrive.endTime);
	DrivingSession.create({
		startTime: startTime,
		endTime: endTime,
		dayDriveTimeTot: calcDayDriveTimeTot(startTime, endTime),
		nightDriveTimeTot: calcNightDriveTimeTot(startTime, endTime),
		distance: distance,
		duration: duration,
		weatherData: {
			temperature: driveSession.DriveWeatherData.temperature,
			summary: driveSession.DriveWeatherData.summary
		}
	}, (err, session)=>{
		if(err){
			error({
				"message": "Error creating driving session",
				"error":err
			});
			return;
		}
		updateUser(userId, session, callback, error);
	});
};

var nineAM;
var fivePM;
var dayHourStart = 9;
var dayHourEnd = 17;
var minPerHour = 60;

function setHourBoundaries(startTime, endTime) {
	nineAM = moment.utc(startTime).hour(dayHourStart).minute(0).second(0); // eslint-disable-line
	fivePM = moment.utc(endTime).hour(dayHourEnd).minute(0).second(0); // eslint-disable-line
}

//TODO: boundary comparisions (i.e <, >, isBefore, isAfter) is being weird
function calcDayDriveTimeTot(startTime, endTime) {
	// Day Hours: 9am (9:00)-5pm (17:00)
	var dayTot = 0;
	setHourBoundaries(startTime, endTime);
	//console.log("should be true: " + (moment.utc(startTime).isAfter(nineAM) && moment.utc(endTime).isBefore(fivePM)));
	if (moment.utc(startTime).isAfter(nineAM) && moment.utc(endTime).isBefore(fivePM)) {
		dayTot = moment(endTime).diff(startTime, "minutes") / minPerHour;
		console.log("dayTot cond1: " + dayTot);
	} else if (moment.utc(startTime).isBefore(fivePM) && moment.utc(endTime).isAfter(fivePM)) {
		dayTot = fivePM.diff(moment.utc(startTime), "minutes") / minPerHour;
		console.log("dayTot cond2: " + dayTot);
	} else if(moment.utc(startTime).isBefore(nineAM) && moment.utc(endTime).isAfter(nineAM)) {
		dayTot = moment.utc(endTime).diff(nineAM, "minutes") / minPerHour;
	}
	return dayTot;
}

function calcNightDriveTimeTot(startTime, endTime) {
	// Night Hours: Before 9:00, After 17:00
	var nightTot = 0;
	// console.log("endTime: " + moment(endTime));
	// console.log("fivePM: " + fivePM);
	// console.log("endTime after 5pm? " + moment.utc(endTime).isAfter(fivePM));
	if ((moment.utc(startTime).isAfter(fivePM) && moment.utc(endTime).isAfter(fivePM)) ||
			(moment.utc(startTime).isBefore(nineAM) && moment.utc(endTime).isBefore(nineAM))) {
		nightTot = moment(endTime).diff(startTime, "minutes") / minPerHour;
		console.log("nightTot cond1: " + nightTot);
	} else if (moment.utc(startTime).isBefore(fivePM) && moment.utc(endTime).isAfter(fivePM)) {
		nightTot = moment.utc(endTime).diff(fivePM, "minutes") / minPerHour;
		console.log("nightTot cond2: " + nightTot);
	} else if (moment.utc(startTime).isBefore(nineAM) && moment.utc(endTime).isAfter(nineAM)) {
		nightTot = nineAM.diff(moment.utc(startTime), "minutes") / minPerHour;
		console.log("nightTot cond3: " + nightTot);
	}
	return nightTot;
}

function closureArray(sizeToRunFunction, fun){
	var a = [];
	return (r) => {
		a.push(r);
		if(a.length >= sizeToRunFunction){
			fun(a);
		}
	};
}

// Welcome to callback hell
// mwhahahahaah
// seriously tho, I will comeback and clean this up, got all these async calls tho
// Ever tried to do an array of async calls? Not fun.
// pinky promise @dylanwalseth
// TODO
exports.createDrivingSessions = function(userId, drivingSessions, callback, error){
	var closure = closureArray(drivingSessions.length, (results)=>{
		var errorResults = results.filter((result) => result.error !== undefined);  // eslint-disable-line
		if(_.any(errorResults)){
			error({
				"message": "One or more driving sessions failed to update",
				"numFailed": errorResults.length,
				"error": errorResults,
				"allResults": closure.array
			});
			return;
		}
		callback(results);
	});
	drivingSessions.forEach((session) => {
		exports.createDrivingSession(userId, session, closure, closure);
	});
};

exports.listDrivingSessions = function(userId, callback, error) {
	User.findOne({
		_id: userId
	}, "drivingSessions")
	.populate("drivingSessions")
	.exec(function(err, drivingSessions) {
		if (err){
			error({
				"message": "Error finding driving sessions",
				"error": err
			});
			return;
		}
		callback(drivingSessions);
	});
};
