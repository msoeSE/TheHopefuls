angular.module("StudentListService", []).factory("students", function($log, $http) {
	var students = {};

	students.getAllUsers = function () {
		return $http.get("/api/allStudents")
		.then(function (response){
			return response;
		});
	};

	return students;
});
