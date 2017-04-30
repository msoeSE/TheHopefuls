var Users = require("../models/User");
var DrivingSessions = require("../models/DrivingSession");

exports.getStudentData = function (studentId, callback, error) {
	//TODO: work on this - @sorianog
	Users.findOne({
		"loginDetails.userId": studentId
	}, function(err, student) {
		if (err) {
			error({
				"message": "Error retrieving student's aggregate driving data",
				"error": err
			});
			return;
		}
		callback(genTotalDrivingData(student));
	});
};

function genTotalDrivingData(student) {
	var dayHours = 0.0;
	var nightHours = 0.0;
	var totalHours = 0.0;

	student.drivingSessions.forEach(function(drive) {
		console.log(drive);
		DrivingSessions.findOne({
			_id: drive
		}, function(err, session) {
			console.log(session.dayDriveTimeTot);
			if (err) {
				error({
					"message": "Error retrieving student's driving session",
					"error": err
				});
				return;
			}
			dayHours += session.dayDriveTimeTot;
			console.log("dayHours: " + dayHours);
			nightHours += session.nightDriveTimeTot;
		});
	});
	console.log("total dayHours: " + dayHours);
	totalHours = dayHours + nightHours;
	var totDrivingData = {
		"dayHours": dayHours,
		"nightHours": nightHours,
		"totalHours": totalHours
	};
	return totDrivingData;
}
