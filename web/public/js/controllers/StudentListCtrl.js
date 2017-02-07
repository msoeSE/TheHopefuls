angular.module("StudentListCtrl", []).controller("StudentListController", function($log, $location) {
	var vm = this;
	vm.tagline = "List of Students for your school!";

	vm.mockStudentData = [
		["1","<a href=\"/stats?id=1\">Nathan Dobbins</a>", "1/08/2017"],
    ["2","<a href=\"/stats?id=2\">Hardip Gill</a>", "1/07/2017"],
    ["3","<a href=\"/stats?id=3\">Gerald Soriano</a>", "1/01/2017"],
    ["4","<a href=\"/stats?id=4\">Dylan Walseth</a>", "1/12/2017"],
    ["5","<a href=\"/stats?id=5\">Patrick Zawadzki</a>", "1/16/2017"]];

	$log.log(vm);
	$log.log($location);

	var table = $(".student-list-table").DataTable({
		data: vm.mockStudentData,
		columns: [
			{ title: "studentID", visible: false},
			{ title: "Student Name" },
			{ title: "Last Log Date" },
			{ title: "Action",
				defaultContent: "<button class = 'waves-effect waves-light btn' type=\"button\">Remove</button>",
				width: "15%"}// Is this the best way to do this?
		]
	});

	$(".btn").on("click", function (event){
		//table.row($(this).parents("tr")).remove().draw();
		// TODO Remove from DB
	});

});
