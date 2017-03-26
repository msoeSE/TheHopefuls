angular.module("StatsService", []).factory("Stats", function($http) {
	var Stats = {};

	Stats.getDriveData = function (userID) {// TODO get user id in a better way?
		return $http.get("/api/students/{}/drivingSessions".format(userID))
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


	return Stats;
});
