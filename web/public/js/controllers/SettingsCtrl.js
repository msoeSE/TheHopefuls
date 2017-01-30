angular.module("SettingsCtrl", ["SettingsService"]).controller("SettingsController", function($log, SettingsOb) {
	var vm = this;

	angular.element(document).ready(function() {
		$("select").material_select();// need to find out why materialize logos and this dont work
	});

	SettingsOb.getProfile().then(function(settings){
		$log.log(settings);
		vm.username = settings.first_name + " " + settings.last_name;
	});


});
