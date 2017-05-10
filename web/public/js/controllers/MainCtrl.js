angular.module("MainCtrl", ["SettingsService"]).controller("MainController", function($location, $window, SettingsOb) {
	var vm = this;

	if($location.url() === "/"){
		SettingsOb.getProfile().then(function (response){
			if(response.userType === "student"){
				$window.location.href = "/stats";
			} else {
				$window.location.href = "/students";
			}
		});
	}
});
