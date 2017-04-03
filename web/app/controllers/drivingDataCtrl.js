var Users = require("../models/User");

exports.getStudentData = function (studentId, callback, error) {
	//TODO: work on this - @sorianog
	Users.findOne({
		loginDetails: { userId: studentId }
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
	var totalHours = 0;
	for (var i in student.drivingSessions) {
		totalHours += student.drivingSessions[i];
	};
	var totDrivingData = {
		"dayHours": 0.0,
		"nightHouts": 0.0,
		"totalHours": totalHours
	};
	return totDrivingData;
}
