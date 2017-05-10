angular.module("StatsCtrl", ["StatsService", "SettingsService"]).controller("StatsController", function($log, $location, Stats, SettingsOb) {  //eslint-disable-line
	var vm = this;
	vm.tagline = "User Stats!";
	if (SettingsOb.state === undefined) { //eslint-disable-line
		vm.state = "Wisconsin";
	} else {
		vm.state = SettingsOb.state;
	}

	var userID = $location.search().id;
	if(userID != null){
		renderTable(userID);
		getTotalDrivingData(userID, vm.state);
	} else {
		Stats.getMongoID().then(function(currentUserID){
			renderTable(currentUserID);
			getTotalDrivingData(currentUserID, vm.state);
		});
	}

	function renderTable(id){
		$(".student-stats-table").DataTable({
			ajax: {url: "/api/students/" + id +  "/drivingSessions",
				dataSrc: "drivingSessions" },
			columns: [
				{ title: "Date", data: "startTime",
					render: function(data, type, full, meta) {
						return moment(full.startTime).format("MM/DD/YYYY");
					}
				},
				{ title: "Start Time",
					render: function(data, type, full, meta) {
						return moment(full.startTime).format("h:mm:ss a");
					}
				},
				{ title: "End Time",
					render: function(data, type, full, meta) {
						return moment(full.endTime).format("h:mm:ss a");
					}
				},
				{ title: "Duration",
					render: function(data, type, full, meta) {
						var date = moment.duration(full.duration);
						var output = " ";
						if(date.hours() !== 0) output += date.hours() + " hours ";//eslint-disable-line
						if(date.minutes() !== 0) output += date.minutes() + " minutes ";//eslint-disable-line
						output += date.seconds() + " seconds ";
						return output;
					}
				},
				{ title: "Distance", data: "distance" },
				{ title: "Temperature", data: "weatherData.temperature" },
				{ title: "Weather Summary", data: "weatherData.summary" }
			]
		});
	}


	function getTotalDrivingData(id, state) { //eslint-disable-line

		//TODO hide flag for states that have no required driving hours
		vm.showHideMessage = false;
		vm.showDayHours = true;
		vm.showNightHours = true;
		vm.showTotalHours = true;

		Stats.getStateRegs(state).then(function(stateRegs) { //eslint-disable-line
			if (stateRegs.dayHours === 0) { //eslint-disable-line
				vm.showDayHours = false;
			} else {
				vm.stateRegTotal = vm.stateRegDay;
			}

			if (stateRegs.nightHours === 0) { //eslint-disable-line
				vm.showNightHours = false;
			} else {
				vm.stateRegTotal = vm.stateRegNight;
			}

			if (stateRegs.dayHours === 0 && stateRegs.nightHours === 0) { //eslint-disable-line
				vm.showHideMessage = true;
				vm.showTotalHours = false;
			}

			vm.stateRegDay = stateRegs.dayHours;
			vm.stateRegNight = stateRegs.nightHours;
			vm.stateRegTotal = vm.stateRegDay + vm.stateRegNight;

		});


		Stats.getTotalDriveData(id).then(function(aggregatData) { //eslint-disable-line
			vm.totDayHours = aggregatData.dayHours;
			vm.dayProgress = (vm.totDayHours / vm.stateRegDay) * 100; //eslint-disable-line

			vm.totNightHours = aggregatData.nightHours;
			vm.nightProgress = (vm.totNightHours / vm.stateRegNight) * 100; //eslint-disable-line

			vm.totDriveHours = aggregatData.totalHours;
			vm.totalProgress = (vm.totDriveHours / vm.stateRegTotal) * 100; //eslint-disable-line
		});
	}

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

	$(".mock-table").DataTable({
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
