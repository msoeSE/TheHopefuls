var DrivingSchool = require("../models/DrivingSchool");
var _ = require("underscore");
var of = require("../libs/objectFunctions");

// TODO: Check if Address, State, and Zip are valid (maybe some regex or external lib)
exports.createSchool = function(info, callback, error) {
	var requiredItems = ["addressLine1", "state", "zip"];
	var missingItems = of.MissingProperties(info, requiredItems);

	if (_.any(missingItems)) {
		error({
			"message": "Required fields for creating school are missing",
			"missing-items": missingItems
		});
		return;
	}

	DrivingSchool.create({
		addressLine1: info.addressLine1,
		addressLine2: info.addressLine2,
		state: info.state,
		zip: info.zip
	}, function(err, newSchool) {
		if (err) {
			error({
				"message": "Error creating a new school",
				"error": err
			});
			return;
		}
		callback(newSchool);
	});
};


exports.getSchool = function(schoolId, callback, error) {
	DrivingSchool.findOne({
		schoolId: schoolId
	}, function(err, doc) {
		if (err) {
			error({
				"message": "Error finding school",
				"error": err
			});
			return;
		}
		callback(doc);
	});
};
