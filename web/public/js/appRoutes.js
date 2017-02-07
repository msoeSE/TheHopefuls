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

		.when("/students", {
			templateUrl: "views/students.html",
			controller: "StudentListController as vm"
		})

		.when("/settings", {
			templateUrl: "views/settings.html",
			controller: "SettingsController as vm"
		})

		.when("/instructors", {
			templateUrl: "views/instructors.html",
			controller: "InstructorListController as vm"
		})

		.otherwise({ redirectTo: "/" });

		$locationProvider.html5Mode(true);
	}
);
