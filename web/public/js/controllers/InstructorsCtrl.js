angular.module("InstructorsCtrl", ["InstructorsService"])
.controller("InstructorListController", function($log, $location, instructorsOb) {
	var vm = this;
	vm.tagline = "List of School Instructors";

	var table = $(".instructor-list-table").DataTable({
		ajax: "/api/instructors",
		initComplete: function () {
			$(".removeBtn").on("click", function (){
				var userId = table.row($(this).parents("tr")).data().userId; // eslint-disable-line
				var schoolId = 0;
				instructorsOb.removeInstructor(schoolId, userId).then(function(results){
					$log.log(results);
					// TODO remove user from table if successful
					//table.row($(this).parents("tr")).remove().draw(); // eslint-disable-line
				});
			});
		},
		columns: [
			{ title: "InstructorID", data: "_id", visible: false },
			{ title: "Instructor Name", data: "firstName",
				render: function(data, type, full, meta){
					return full.firstName + " " + full.lastName;
				}},
			{ title: "Action",
				defaultContent:
					"<button class = 'waves-effect waves-light btn removeBtn' type=\"button\">Remove</button>",
				width: "15%" }
		]
	});


	// Mocked Instructor Data

	vm.mockInstructorData = [
		["1","Nathan Dobbins"],
    ["2","Hardip Gill"],
    ["3","Gerald Soriano"],
    ["4","Dylan Walseth"],
    ["5","Patrick Zawadzki"]];

	var mocktable = $(".mock-table").DataTable({
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

	$("#addBtn").on("click", function (){
		var schoolId = 0;
		var userId = $("#instructorID").val();
		instructorsOb.addInstructor(schoolId, userId).then(function (result){
				$log.log(results);
				// TODO add user to table if successful
				//table.row.add([id, $("#instructorID").val()]).draw(false);
		});
		$("#instructorID").val("");
	});

});
