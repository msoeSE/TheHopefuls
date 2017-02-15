angular.module("InstructorsCtrl", []).controller("InstructorListController", function($log, $location) {
	var vm = this;
	vm.tagline = "List of School Instructors";

	vm.mockInstructorData = [
		["1","Nathan Dobbins"],
    ["2","Hardip Gill"],
    ["3","Gerald Soriano"],
    ["4","Dylan Walseth"],
    ["5","Patrick Zawadzki"]];

	$log.log(vm);
	$log.log($location);

	var table = $(".instructor-list-table").DataTable({
		data: vm.mockInstructorData,
		columns: [
			{ title: "Instructor ID", visible: false},
			{ title: "Instructor Name" },
			{ title: "Action",
				defaultContent:
					"<button class = 'waves-effect waves-light btn removeBtn' type=\"button\">Remove</button>",
				width: "15%"}
		]
	});

	$(".removeBtn").on("click", function (){
		table.row($(this).parents("tr")).remove().draw(); // eslint-disable-line
		// TODO Remove from DB
	});

	$("#addBtn").on("click", function (){
		var id = 0;
		table.row.add([id, $("#name").val()]).draw(false);
		$("#name").val("");
		// TODO Add to database
	});

});
