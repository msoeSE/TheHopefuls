var User = require("../models/User");

exports.createUser = function(info, userType, callback, error) {
	var requiredItems = ["firstName", "lastName", "loginDetails"];
	var missingItems = info.missingProperties(requiredItems);

	if(missingItems.any()){
		error({
			"message": "Required fields for creating student are missing",
			"missing-items": missingItems
		});
		return;
	}

	var newStudent = new User({
		loginDetails: {
			userId: info.loginDetails.userId,
			service: info.loginDetails.service
		},
		firstName: info.firstName,
		lastName: info.lastName,
		userType: userType
	});

	newStudent.save(function(err) {
		if (err) {
			error({
				"message": "Error creating new user",
				"error": err
			});
			return;
		}
		callback(newStudent);
	});
};

exports.getUser = function(id, callback, error){
	User.findOne({
		_id: id
	}, function(err, doc) {
		if (err) {
			error({
				"message": "Error finding user",
				"error": err
			});
			return;
		}
		callback(doc);
	});
};

exports.createStudent = (info, callback, error) => exports.createUser(info, "student", callback, error);
exports.createInstructor = (info, callback, error) => exports.createUser(info, "instructor", callback, error);
exports.getStudent = exports.getUser;
exports.getInstructor = exports.getUser;
