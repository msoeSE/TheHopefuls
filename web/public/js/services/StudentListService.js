angular.module("StudentListService", []).factory("students", function($log, $http) {
	var vm = this;

	// TODO write this request when the api has it
	$http.get("")
	.then(function(response) {
		vm.studentsData = response;
	});
});
