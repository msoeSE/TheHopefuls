angular.module("StatsService", []).factory("Stats", function($http) {
	var Stats = {};

	Stats.getDriveData = function (userID) {// TODO get user id in a better way?
		return $http.get("/api/students/" + userID + "/drivingSessions")
		.then(function (response){
			return response;
		});
	};

	Stats.getUserID = function () {
		return $http.get("/profile")
		.then(function (response){
			return response.data.id;
		});
	};

	Stats.getMongoID = function () {
		return $http.get("/profile")
		.then(function (response){
			return response.data.mongoID;
		});
	};

	Stats.getTotalDriveData = function (userID) {
		return $http.get("api/totalDrivingData/" + userID)
		.then(function (response) {
			return response.data;
		});
	};

	Stats.getStateRegs = function (state) {
		return $http.get("/api/stateregulations/" + state)
		.then(function (response) {
			return response.data;
		});
	};

	return Stats;
});
