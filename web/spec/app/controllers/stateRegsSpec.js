var StateRegulations = require(`${process.env.PWD}/app/models/StateRegulations`);
var toTest = require(`${process.env.PWD}/app/controllers/stateRegsCtrl`);


describe("State Regulations Endpoint Tests", function() {
	var success;
	var failure;
	var shouldErr;
	var stateRegsMockObject = {
		"Object": "MockedSchool"
	};
	beforeEach(function() {
		shouldErr = undefined;
		success = jasmine.createSpy("success");
		failure = jasmine.createSpy("failure");
	});

	describe("getStateRegs", function() {
		beforeEach(function() {
			spyOn(StateRegulations, "findOne").andCallFake(function(_, callback) {
				if (shouldErr) {
					callback("error", null);
				} else {
					callback(null, stateRegsMockObject);
				}
			});
		});
		it("should get a state regulations object if there is no database error", function() {
			shouldErr = false;
			toTest.getStateRegs("state", success, failure);
			expect(success).toHaveBeenCalledWith(stateRegsMockObject);
			expect(failure).not.toHaveBeenCalled();
		});
		it("should get an error if there is a database error", function(){
			shouldErr = true;
			toTest.getStateRegs("state", success, failure);
			expect(success).not.toHaveBeenCalled();
			expect(failure).toHaveBeenCalledWith({
				"message": "Error retrieving state regulations",
				"error": "error"
			});
		});
	})
});
