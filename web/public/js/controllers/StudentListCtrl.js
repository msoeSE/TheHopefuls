angular.module("StudentListCtrl", ["StatsCtrl"])
.controller("StudentListController", function($log, $location, students) {
	var vm = this;
	vm.tagline = "List of Students for your school!";

	$(".student-list-table").DataTable({
		ajax: "/api/allStudents",
		columns: [
			{ title: "studentID", data: "_id", visible: false },
			{ title: "Student Name", data: "firstName",
				render: function(data, type, full, meta){
					return "<a href=\"/stats?id=" + full._id + "\">" +
						full.firstName + " " + full.lastName + "</a>";
				}},
			{ title: "Last Log Date",
				render: function(data, type, full, meta){
					return "-";
				},
				defualtContent: "-"
			},
			{ title: "Action",
				defaultContent: "<button class = 'waves-effect waves-light btn' type=\"button\">Remove</button>",
				width: "15%"}// Is this the best way to do this?
		]
	});

	$(".btn").on("click", function (){
		// table.row($(this).parents("tr")).remove().draw();
		// TODO Remove from DB
	});

	// Mocked Data Stuff

	vm.mockStudentData = [
		["1","<a href=\"/stats?id=1\">Nathan Dobbins</a>", "1/08/2017"],
    ["2","<a href=\"/stats?id=2\">Hardip Gill</a>", "1/07/2017"],
    ["3","<a href=\"/stats?id=3\">Gerald Soriano</a>", "1/01/2017"],
    ["4","<a href=\"/stats?id=4\">Dylan Walseth</a>", "1/12/2017"],
    ["5","<a href=\"/stats?id=5\">Patrick Zawadzki</a>", "1/16/2017"]];

	$(".test-table").DataTable({
		data: vm.mockStudentData,
		columns: [
			{ title: "studentID", visible: false},
			{ title: "Student Name" },
			{ title: "Last Log Date" },
			{ title: "Action",
				defaultContent: "<button class = 'waves-effect waves-light btn' type=\"button\">Remove</button>",
				width: "15%"}
		]
	});
});
