angular.module("SettingsService", []).factory("SettingsOb", function($http) {
	var SettingsOb = {};


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
