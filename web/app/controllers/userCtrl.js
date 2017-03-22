var User = require("../models/User");
var _ = require("underscore");
var of = require("../libs/objectFunctions");


exports.createUser = function(info, userType, callback, error) {
	var requiredItems = ["firstName", "lastName", "loginDetails"];
	var missingItems = of.MissingProperties(info, requiredItems);

	if(_.any(missingItems)){
		error({
			"message": "Required fields for creating student are missing",
			"missing-items": missingItems
		});
		return;
	}

	User.create({
		loginDetails: {
			userId: info.loginDetails.userId,
			service: info.loginDetails.service
		},
		firstName: info.firstName,
		lastName: info.lastName,
		userType: userType
	}, function(err, newUser) {
		if (err) {
			error({
				"message": "Error creating new user",
				"error": err
			});
			return;
		}
		callback(newUser);
	});
};

exports.getUser = function(id, callback, error){
	User.findOne({
		loginDetails: { userId: id }
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
