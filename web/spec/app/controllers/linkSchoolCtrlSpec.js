var User = require(`${process.env.PWD}/app/models/User`);
var toTest = require(`${process.env.PWD}/app/controllers/linkSchoolCtrl`);

describe("Link Account to Driving School Test", function() {
	var success;
	var failure;
	var shouldErr;
	var studentMockObject = {
		"Object": "MockedStudent"
	};
	beforeEach(function() {
		shouldErr = undefined;
		success = jasmine.createSpy("success");
		failure = jasmine.createSpy("failure");
	});

	describe("linkAccToSchool", function() {
		beforeEach(function() {
			spyOn(User, "findOneAndUpdate").andCallFake(function(_, _, callback) {
				if (shouldErr) {
					callback("error", null);
				} else {
					callback(null, studentMockObject);
				}
			});
		});
		it("should link student to school if there is no database error", function() {
			shouldErr = false;
			toTest.linkAccToSchool("userID", "schoolId",success, failure);
			expect(success).toHaveBeenCalledWith(studentMockObject);
			expect(failure).not.toHaveBeenCalled();
		});
		it("should get an error if there is a database error", function(){
			shouldErr = true;
			toTest.linkAccToSchool("userID", "schoolId", success, failure);
			expect(success).not.toHaveBeenCalled();
			expect(failure).toHaveBeenCalledWith({
				"message": "Error finding student to link school with",
				"error": "error"
			});
		});
	});
});
