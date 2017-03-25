angular.module("StatsService", []).factory("Stats", function($http) {
	var Stats = {};

	Stats.test = function(){
		return "testing this.";
	};

	Stats.getDriveData = function (userID) {// TODO get user id?
		return $http.get("/api/students/{}/drivingSessions".format(userID))
		.then(function (response){
			return response;
		});
	};

	Stat.getUserID = function () {
		return $http.get("/profile")
		.then(function (response){
			return response.data.id;
		});
	};


	return Stats;
});
