angular.module("StudentListService", []).factory("students", function($log, $http) {
	var students = {};

	students.getAllUsers = function () {
		return $http.get("/api/allStudents")
		.then(function (response){
			return response;
		});
	};

	students.getSchoolId = function () {
		return $http.get("/profile")
		.then(function (response){
			return response.data.schoolId;
		});
	};

	return students;
});
