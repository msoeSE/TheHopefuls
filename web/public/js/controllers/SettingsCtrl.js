angular.module("SettingsCtrl", ["SettingsService"]).controller("SettingsController", function($log, SettingsOb) {
	var vm = this;
	vm.state = SettingsOb.state;

	SettingsOb.getProfile().then(function(settings){
		vm.username = settings._json.first_name + " " + settings._json.last_name;
		vm.profilePicture = settings._json.picture.data.url;
		vm.instructorID = settings.mongoID;
	});


});
