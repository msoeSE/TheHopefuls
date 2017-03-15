var DrivingSchool = require("../models/DrivingSchool");
var User = require("../models/User");
var drivingSchoolCtrl = require("../controllers/drivingSchoolCtrl");


// TODO: pull driving school, pull user, update user with schoolId for linking and syncing
exports.linkAccToSchool = function (userId, schoolId, callback, error) {
	User.findOneAndUpdate({ userId: userId}, {schoolId: schoolId}, function(err, doc) {
		if (err) {
			error({
				"message": "Error retrieving state regulations",
				"error": err
			});
			return;
		}
		drivingSchoolCtrl.addStudentToSchool(schoolId, doc);
		callback(doc);
	});
};
