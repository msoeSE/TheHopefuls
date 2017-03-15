var DrivingSchool = require(`${process.env.PWD}/app/models/DrivingSchool`);
var toTest = require(`${process.env.PWD}/app/controllers/linkSchoolCtrl`);

describe("Link Account to Driving School Test", function() {
	var success;
	var failure;
	var shouldErr;
	var schoolMockObject = {
		"Object": "MockedSchool"
	};
	beforeEach(function() {
		shouldErr = undefined;
		success = jasmine.createSpy("success");
		failure = jasmine.createSpy("failure");
	});
});
