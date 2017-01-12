var DrivingSchool = require("../models/DrivingSchool");

// TODO: Check if Address, State, and Zip are valid (maybe some regex or external lib)
exports.createSchool = function(info, callback, error) {
	var requiredItems = ["addressLine1", "state", "zip"];
	var missingItems = MissingProperties(info, requiredItems);

	if (_.any(missingItems)) {
		error({
			"message": "Required fields for creating school are missing",
			"missing-items": missingItems
		});
		return;
	}
	var newSchool = new DrivingSchool({
		addressLine1: req.body.addressLine1,
		addressLine2: req.body.addressLine2,
		state: req.body.state,
		zip: req.body.zip
	});
	newSchool.save(function(err) {
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
