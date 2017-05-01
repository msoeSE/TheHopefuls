var User = require(`${process.env.PWD}/app/models/User`);
var toTest = require(`${process.env.PWD}/app/controllers/userCtrl`);

describe("User Controller Tests", function() {
	var success;
	var failure;
	var shouldErr;
	var dummyDatabaseObject = {"Object": "MockedUser"};
	beforeEach(function(){
		shouldErr = undefined;
		success = jasmine.createSpy("success");
		failure = jasmine.createSpy("failure");
	});

	xdescribe("createUser", function() {
		beforeEach(function(){
			spyOn(User, "create").andCallFake(function(_, callback){
				if(shouldErr){
					callback("error", null);
				} else {
					callback(null, dummyDatabaseObject);
				}
			});
		});
		it("should create a new user if required fields are valid", function(){
			shouldErr = false;
			toTest.createUser({
				"firstName": "John",
				"lastName": "Cena",
				"loginDetails": {
					"userId": "1",
					"service": "Funbook"
				}
			}, "test", success, failure);
			expect(User.create).toHaveBeenCalled();
			expect(success).toHaveBeenCalledWith(dummyDatabaseObject);
			expect(failure).not.toHaveBeenCalled();
		});

		it("should not create a new user if a required field is missing", function(){
			shouldErr = false;
			toTest.createUser({
				"lastName": "Cena",
				"loginDetails": {
					"userId": "1",
					"service": "Funbook"
				}
			}, "test", success, failure);
			expect(User.create).not.toHaveBeenCalled();
			expect(success).not.toHaveBeenCalled();
			expect(failure).toHaveBeenCalledWith(jasmine.objectContaining({
				"missing-items": ["firstName"]
			}));
		});

		it("should not create a new user if the database fails to update", function(){
			shouldErr = true;
			toTest.createUser({
				"firstName": "John",
				"lastName": "Cena",
				"loginDetails": {
					"userId": "1",
					"service": "Funbook"
				}
			}, "test", success, failure);
			expect(failure).toHaveBeenCalledWith({
				"message": "Error creating new user",
				"error": "error"
			});
			expect(success).not.toHaveBeenCalled();
		});
	});

	describe("getUser", function() {
		beforeEach(function(){
			spyOn(User, "findOne").andCallFake(function(_, callback){
				if(shouldErr){
					callback("error", null);
				} else {
					callback(null, dummyDatabaseObject);
				}
			});
		});
		it("should get the user if there is no database error", function(){
			shouldErr = false;
			toTest.getUser("id", success, failure);
			expect(success).toHaveBeenCalledWith(dummyDatabaseObject);
			expect(failure).not.toHaveBeenCalled();
		});
		it("should get an error if there is a database error", function(){
			shouldErr = true;
			toTest.getUser("id", success, failure);
			expect(success).not.toHaveBeenCalled();
			expect(failure).toHaveBeenCalledWith({
				"message": "Error finding user",
				"error": "error"
			});
		});
	});
});
