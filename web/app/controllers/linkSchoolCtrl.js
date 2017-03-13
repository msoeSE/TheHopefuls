var DrivingSchool = require("../models/DrivingSchool");
var User = require("../models/User");

// TODO: pull driving school, pull user, update user with schoolId for linking and syncing
exports.linkAccToSchool = function () {
	User.findOneAndUpdate({ userId: userId}, {schoolId: schoolId}, function(err, doc) {
		if (err) {
			error({
				"message": "Error retrieving state regulations",
				"error": err
			});
			return;
		}
		callback(doc);
	});
};
