angular.module("InstructorsService", []).factory("instructorsOb", function($http) {
	var instructorsOb = {};

	instructorsOb.addInstructor = function (schoolID, userID) {
		return $http.post("/api/drivingschools/" + schoolID + "/instructors", userID)
		.then(function (response){
			return response;
		});
	};

	instructorsOb.removeInstructor = function (schoolID, userID) {
		return $http.delete("/api/drivingschools/" + schoolID + "/instructors/" + userID)
		.then(function (response){
			return response;
		});
	};

	instructorsOb.getProfile = function () {
		return $http.get("/profile")
		.then(function (response){
			return response.data;
		});
	};

	return instructorsOb;
});
