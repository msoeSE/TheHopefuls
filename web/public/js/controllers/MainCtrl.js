angular.module("MainCtrl", ["SettingsService"]).controller("MainController", function($location, $window, SettingsOb) {
	var vm = this;

	if($location.url() === "/" || $location.url() === "#_=_"){
		SettingsOb.getProfile().then(function (response){
			if(response.userType === "student"){
				$window.location.href = "/stats";
			} else {
				$window.location.href = "/students";
			}
		});
	}

	SettingsOb.getProfile().then(function (response){
		if(response.userType === "student"){
			$("#students").hide();
			$("#instructors").hide();
		} else if (response.userType === "instructor"){
			$("#stats").hide();
			$("#instructors").hide();
		} else {
			$("#stats").hide();
		}
	});

});
