var DrivingSchool = require("../../../app/models/DrivingSchool");
var toTest = require("../../../app/controllers/drivingSchoolCtrl");

describe("Driving School Controller Tests", function() {
	var success;
	var failure;
	var shouldErr;
	var dummyDatabaseObject = {"Object": "MockedSchool"};
	beforeEach(function(){
		shouldErr = undefined;
		success = jasmine.createSpy("success");
		failure = jasmine.createSpy("failure");
	});

	describe("createSchool", function() {
		beforeEach(function(){
			spyOn(DrivingSchool, "create").andCallFake(function(_, callback){
				if(shouldErr){
					callback("error", null);
				} else {
					callback(null, dummyDatabaseObject);
				}
			});
		});
		it("should create a new school if required fields are valid", function(){
			shouldErr = false;
			toTest.createSchool({
				"addressLine1": "255 Bitwise Lane",
				"zip": "53202",
				"state": "WI"
			}, success, failure);
			expect(DrivingSchool.create).toHaveBeenCalled();
			expect(success).toHaveBeenCalledWith(dummyDatabaseObject);
			expect(failure).not.toHaveBeenCalled();
		});

		it("should not create a new school if a required field is missing", function(){
			shouldErr = false;
			toTest.createSchool({
				"addressLine1": "255 Bitwise Lane",
				"state": "WI"
			}, success, failure);
			expect(failure).toHaveBeenCalledWith(jasmine.objectContaining({
				"missing-items": ["zip"]
			}));
			expect(DrivingSchool.create).not.toHaveBeenCalled();
			expect(success).not.toHaveBeenCalled();
		});

		it("should not create a new school if the database fails to update", function(){
			shouldErr = true;
			toTest.createSchool({
				"addressLine1": "255 Bitwise Lane",
				"zip": "53202",
				"state": "WI"
			}, success, failure);
			expect(failure).toHaveBeenCalledWith({
				"message": "Error creating a new school",
				"error": "error"
			});
			expect(success).not.toHaveBeenCalled();
		});
	});

	describe("getSchool", function() {
		beforeEach(function(){
			spyOn(DrivingSchool, "findOne").andCallFake(function(_, callback){
				if(shouldErr){
					callback("error", null);
				} else {
					callback(null, dummyDatabaseObject);
				}
			});
		});
		it("should get the school if there is no database error", function(){
			shouldErr = false;
			toTest.getSchool("id", success, failure);
			expect(success).toHaveBeenCalledWith(dummyDatabaseObject);
			expect(failure).not.toHaveBeenCalled();
		});
		it("should get an error if there is a database error", function(){
			shouldErr = true;
			toTest.getSchool("id", success, failure);
			expect(success).not.toHaveBeenCalled();
			expect(failure).toHaveBeenCalledWith({
				"message": "Error finding school",
				"error": "error"
			});
		});
	});
});
