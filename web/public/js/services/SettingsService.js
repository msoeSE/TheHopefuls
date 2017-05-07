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

	return SettingsOb;
});
