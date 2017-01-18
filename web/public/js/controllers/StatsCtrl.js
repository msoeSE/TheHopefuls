angular.module("StatsCtrl", []).controller("StatsController", function($log, $location) {
	var vm = this;
	vm.tagline = "User Stats!";

	// TODO right now we arent loading the users data, by we can get the
	// query string for it. Change this to use database data loaded from
	// the users ID
	$log.log($location.search().id);

	vm.mockUserData = [
		["10/10/2016", "1:00pm", "2:00pm", "1 Hr", "23 Miles", "Rainy"],
		["10/10/2016", "4:00pm", "5:00pm", "1 Hr", "45 Miles", "Rainy"],
		["11/16/2016", "3:00pm", "4:00pm", "1 Hr", "37 Miles", "Rainy"],
		["12/16/2016", "7:00pm", "8:00pm", "1 Hr", "60 Miles", "Rainy"],
		["11/20/2016", "2:00pm", "3:00pm", "1 Hr", "5 Miles", "Snowy"],
		["11/25/2016", "5:00pm", "6:00pm", "1 Hr", "18 Miles", "Snowy"],
		["11/03/2016", "6:00pm", "7:00pm", "1 Hr", "39 Miles", "Snowy"],
		["10/04/2016", "8:00pm", "9:00pm", "1 Hr", "42 Miles", "Snowy"],
		["10/13/2016", "9:00pm", "10:00pm", "1 Hr", "49 Miles", "Windy"],
		["10/19/2016", "10:00pm", "11:00pm", "1 Hr", "21 Miles", "Windy"],
		["09/02/2016", "11:00pm", "12:00am", "1 Hr", "55 Miles", "Windy"],
		["09/16/2016", "1:00am", "2:00am", "1 Hr", "16 Miles", "Windy"],
		["09/17/2016", "2:00am", "3:00am", "1 Hr", "28 Miles", "Breezy"],
		["06/04/2016", "3:00am", "4:00am", "1 Hr", "31 Miles", "Breezy"],
		["06/06/2016", "4:00am", "5:00am", "1 Hr", "22 Miles", "Sunny"],
		["06/30/2016", "5:00am", "6:00am", "1 Hr", "88 Miles", "Sunny"],
		["07/04/2016", "6:00am", "7:00am", "1 Hr", "11 Miles", "Hurricane"]
	];

	$log.log(vm);


	angular.element(".student-stats-table").DataTable({
		data: vm.mockUserData,
		columns: [
			{ title: "Date" },
			{ title: "Start Time" },
			{ title: "End Time" },
			{ title: "Duration" },
			{ title: "Distance" },
			{ title: "Weather" }
		]
	});


});
