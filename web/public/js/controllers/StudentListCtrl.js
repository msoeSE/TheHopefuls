angular.module("StudentListCtrl", []).controller("StudentListController", function($log, $location) {
	var vm = this;
	vm.tagline = "List of Students for your school!";

	vm.mockStudentData = [
		["0","<a href=\"/stats?0\">Nathan Dobbins</a>", "1/08/2017"],
    ["1","Hardip Gill", "1/07/2017"],
    ["2","Gerald Soriano", "1/01/2017"],
    ["3","Dylan Walseth", "1/12/2017"],
    ["4","Patrick Zawadzki", "1/16/2017"]];

	$log.log(vm);

	$log.log($location)

	var table = $(".student-list-table").DataTable({
		data: vm.mockStudentData,
		columns: [
			{ title: "studentID", visible: false},
			{ title: "Student Name" },
			{ title: "Last Log Date" },
			{ title: "Action",
				defaultContent: "<button type=\"button\">Remove</button>"}// Is this the best way to do this?
		]
	});

	// table.on("click", "tr", function() {
	// 	var selectedRow = this;
	// 	// redirect to stats page here?
	// 	// would have to account for remove button
	// 	if ($(selectedRow).hasClass("selected")) {
	// 		$(selectedRow).removeClass("selected");
	// 	} else {
	// 		table.$("tr.selected").removeClass("selected");
	// 		$(selectedRow).addClass("selected");
	// 	}
	// });
});
