var DrivingSession = require("../models/DrivingSession");
var User = require("../models/User");

// TODO: find an accurate formula for distance from gps points
// TODO 2: More likely, use Google maps, depends on what mobile is doing and
// what we can expect the data to look like
function calculateDistance(drivePoints) {
	// 0 for now
	var distance = 0;
	// drivePoints obj has lat, lon, speed, time
	return distance;
}

// Diff in time in Milliseconds
// Takes in two MomentJS objects
function calculateDuration(startTime, endTime) {
	return endTime.diff(startTime);
}


// TODO Do to this file what I did to the other two. Too tired for rn
function updateStudentDrive(userId, newDrivingSession, res) {
	User.findOne({
		_id: userId
	}, function(err, student) {
		if (!err) {
			student.drivingSessions.push(newDrivingSession);
			student.save(function(saveErr) {
				if (saveErr) {
					res.send("Error saving driving session to student, " + saveErr);
				}
			});
		} else {
			res.send("Error retrieving driving session data, " + err);
		}
	});
}

function checkCreateRequest(newDrivingSession, res) {
	let badRequest = 400; // TODO: Remove
	let created = 201;
	if (!newDrivingSession.startTime) {
		res.status(badRequest);
		res.send("Start time required!");
	} else if (!newDrivingSession.endTime) {
		res.status(badRequest);
		res.send("End time required!");
	} else {
		newDrivingSession.save(function(err) {
			if (!err) {
				res.status(created);
				res.json(newDrivingSession);
			} else {
				json.send("Error saving driving session, " + err);
			}
		});
	}
}

exports.createDrivingSession = function(req, res) {
	driveSessions = req.body;
	driveSessions.forEach(function(driveSession) {
		var finalDistance = calculateDistance(driveSession.DrivePoints);
		var finalDuration = calculateDuration(driveSession.UnsyncDrive.startTime,
			driveSession.UnsyncDrive.endTime);
		newDrivingSession = new DrivingSession({
			startTime: new Date(driveSession.UnsyncDrive.startTime),
			endTime: new Date(driveSession.UnsyncDrive.endTime),
			distance: finalDistance,
			//TODO: figure out what units are
			duration: finalDuration,
			weatherData: {
				temperature: driveSession.DriveWeatherData.temperature,
				summary: driveSession.DriveWeatherData.summary
			}
		});

		checkCreateRequest(newDrivingSession, res);
		updateStudentDrive(req.params.userId, newDrivingSession, res);
	});
};

exports.listDrivingSessions = function(req, res) {
	User.findOne({
			_id: req.params.userId
		}, "drivingSessions")
		.populate("drivingSessions").exec(function(err, drivingSessions) {
			if (!err) {
				res.statusCode = 200;
				res.json(drivingSessions);
			} else {
				res.send("Error retrieving driving session data, " + err);
			}
		});
};
