angular.module("SettingsCtrl", ["SettingsService"]).controller("SettingsController", function($log, SettingsOb) {
	var vm = this;

	SettingsOb.getProfile().then(function(settings){
		vm.username = settings._json.first_name + " " + settings._json.last_name;
		vm.profilePicture = settings._json.picture.data.url;
		vm.instructorID = settings.mongoID;
		if(settings.schoolId !== 0){//eslint-disable-line
			vm.schoolId = settings.schoolId;
		}
	});

	SettingsOb.getStates().then(function(states){
		$.each(states.data, function(){
			$("#stateSelect").append(
				$("<option></option>").text(this.state).val(this._id)//eslint-disable-line
			);
		});
	});

});
