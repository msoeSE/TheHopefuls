angular.module("SettingsCtrl", ["SettingsService"]).controller("SettingsController", function($log, SettingsOb) {
	var vm = this;

	SettingsOb.getProfile().then(function(settings){
		vm.username = settings.first_name + " " + settings.last_name;
		vm.profilePicture = settings.picture.data.url;
	});


});
