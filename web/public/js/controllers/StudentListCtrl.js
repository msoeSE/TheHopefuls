angular.module("StudentListCtrl", ["StudentListService"])
.controller("StudentListController", function($log, $location, students) {
	var vm = this;
	vm.tagline = "List of Students for your school!";

	students.getSchoolId().then(function (schoolId){
		createTable(schoolId);
	});

	function createTable(schoolId) {
		$(".student-list-table").DataTable({
			ajax: "/api/drivingschools/" + schoolId + "/students",
			columns: [
				{ title: "studentID", data: "_id", visible: false },
				{ title: "Student Name", data: "firstName",
					render: function(data, type, full, meta){
						return "<a href=\"/stats?id=" + full._id + "\">" +
							full.firstName + " " + full.lastName + "</a>";
					}
				},
				{ title: "Action",
					defaultContent: "<button class = 'waves-effect waves-light btn' type=\"button\">Remove</button>",
					width: "15%"}// Is this the best way to do this?
			]
		});
	}

	$(".btn").on("click", function (){
		table.row($(this).parents("tr")).remove().draw();//eslint-disable-line
		// TODO Remove from DB
	});

});
