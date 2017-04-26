var User = require("../models/User");
var drivingSchoolCtrl = require("../controllers/drivingSchoolCtrl");


// update user with schoolId for linking
exports.linkAccToSchool = function(userId, schoolId, callback, error) {
	drivingSchoolCtrl.getSchool(schoolId,
	(results) => {
		if(results != null){
			User.findOneAndUpdate({
				userId: userId
			}, {
				schoolId: schoolId
			}, function(err, student) {
				if (err) {
					error({
						"message": "Error finding student to link school with",
						"error": err
					});
					return;
				}
				drivingSchoolCtrl.addStudentToSchool(schoolId, student,
					(school) => {
						callback({student: student, school: school});
					}, (schoolError) => {
						callback(schoolError);
					});
			});
		} else {
			callback({error: "No school found"});
		}
	},
	(err) => {
		callback(err);
	});
};
