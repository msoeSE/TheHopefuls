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

	return SettingsOb;
});
