angular.module("StudentListCtrl", []).controller("StudentListController", function($log) {
	var vm = this;
	vm.tagline = "List of Students for your school!";

	vm.mockStudentData = [
		["Nathan Dobbins", "1/08/2017"],
    ["Hardip Gill", "1/07/2017"],
    ["Gerald Soriano", "1/01/2017"],
    ["Dylan Walseth", "1/12/2017"],
    ["Patrick Zawadzki", "1/16/2017"]];

	$log.log(vm);


	var table = $(".student-list-table").DataTable({
		data: vm.mockStudentData,
		columns: [
			{ title: "Student Name" },
			{ title: "Last Log Date" },
			{ title: "Action",
				defaultContent: "<button type=\"button\">Remove</button>"}// Is this the best way to do this?
		]
	});

	table.on("click", "tr", function() {
		// redirect to stats page here?
		// would have to account for remove button
		if ($(this).hasClass("selected")) {
			$(this).removeClass("selected");
		} else {
			table.$("tr.selected").removeClass("selected");
			$(this).addClass("selected");
		}
	});
});
