angular.module("InstructorsCtrl", []).controller("InstructorListController", function($log, $location) {
	var vm = this;
	vm.tagline = "List of School Instructors";

	// vm.mockInstructorData = [
	// 	["0","Nathan Dobbins"],
  //   ["1","Hardip Gill"],
  //   ["2","Gerald Soriano"],
  //   ["3","Dylan Walseth"],
  //   ["4","Patrick Zawadzki"]];
	//
	// $log.log(vm);
	// $log.log($location);
	//
	// var table = $(".instructor-list-table").DataTable({
	// 	data: vm.mockInstructorData,
	// 	columns: [
	// 		{ title: "Instructor ID", visible: false},
	// 		{ title: "Instructor Name" },
	// 		{ title: "Action",
	// 			defaultContent: "<button type=\"button\">Remove</button>"}// Is this the best way to do this?
	// 	]
	// });

});
