angular.module("appRoutes", []).config(
	function($routeProvider, $locationProvider) {

		$routeProvider
		.when("/", {
			templateUrl: "views/home.html",
			controller: "MainController as vm"
		})

		.when("/stats", {
			templateUrl: "views/stats.html",
			controller: "StatsController as vm"
		})

		.otherwise({ redirectTo: "/" });

		$locationProvider.html5Mode(true);
	}
);
