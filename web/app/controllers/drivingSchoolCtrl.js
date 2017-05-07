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

// TODO: Add update method to add a student to be added to the school
// WHAT DOES THIS EVEN MEAN?!? ^^^^^
// I read this like 5 times and can't figure it out
exports.addStudentToSchool = function(schoolId, student, callback, error) {
	DrivingSchool.findOneAndUpdate({
		schoolId: schoolId
	}, {$push: {students: student}},
	function(err, doc) {
		if (err) {
			error({
				"message": "Error adding student to driving school",
				"error": err
			});
			return;
		}
		callback(doc);
	});
};

exports.addInstructorToSchool = function(schoolId, instructor, callback, error) {
	DrivingSchool.findOneAndUpdate({
		schoolId: schoolId
	}, {$push: {instructors: instructor}},
	function(err, doc) {
		if (err) {
			error({
				"message": "Error adding instructor to driving school",
				"error": err
			});
			return;
		}
		callback(doc);
	});
};

exports.removeStudentFromSchool = function(schoolId, student, callback, error) {
	DrivingSchool.findOneAndUpdate({
		schoolId: schoolId
	}, {$pull: {students: student}},
	function(err, doc) {
		if (err) {
			error({
				"message": "Error removing student from driving school",
				"error": err
			});
			return;
		}
		callback(doc);
	});
};

exports.removeInstructorFromSchool = function(schoolId, instructor, callback, error) {
	DrivingSchool.findOneAndUpdate({
		schoolId: schoolId
	}, {$pull: {instructors: instructor}},
	function(err, doc) {
		if (err) {
			error({
				"message": "Error adding student to driving school",
				"error": err
			});
			return;
		}
		callback(doc);
	});
};
