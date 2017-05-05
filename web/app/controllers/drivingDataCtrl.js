var Users = require("../models/User");

exports.getStudentData = function (studentId, callback, error) {
	Users.findOne({
		_id: studentId
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
	var drivingSessions = student.drivingSessions;
	var dayHours = 0.0;
	var nightHours = 0.0;
	var totalHours = 0.0;

	for(var i = 0; i < drivingSessions.length; i++) {
		dayHours += drivingSessions[i].dayDriveTimeTot;
		nightHours += drivingSessions[i].nightDriveTimeTot;
	}

	totalHours = dayHours + nightHours;
	var totDrivingData = {
		"dayHours": dayHours,
		"nightHours": nightHours,
		"totalHours": totalHours
	};
	return totDrivingData;
}
