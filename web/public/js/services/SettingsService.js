angular.module("SettingsService", []).factory("SettingsOb", function($http) {
	var SettingsOb = {};
	var state = "Wisconsin";
	SettingsOb.getState = function() {
		return state;
	};

	SettingsOb.setState = function(stateSelected) {
		state = stateSelected;
	};

	SettingsOb.getProfile = function () {
		return $http.get("/profile")
		.then(function (response){
			return response.data;
		});
	};

	SettingsOb.getStates = function () {
		return $http.get("/api/states")
		.then(function (response){
			return response;
		});
	};

	SettingsOb.updateSchoolCode = function (schoolId, userId) {
		return $http.post("/api/linkacctoschool", {schoolId: schoolId, userId: userId})
		.then(function (response){
			return response;
		});
	};

	return SettingsOb;
});
