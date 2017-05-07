angular.module("SettingsCtrl", ["SettingsService"]).controller("SettingsController", function($log, SettingsOb) {
	var vm = this;

	SettingsOb.getProfile().then(function(settings){
		vm.username = settings._json.first_name + " " + settings._json.last_name;
		vm.profilePicture = settings._json.picture.data.url;
		vm.instructorID = settings.mongoID;
		if(settings.schoolId !== 0){
			vm.schoolId = settings.schoolId;
		}
	});

	SettingsOb.getStates().then(function(states){
		console.log(states);
		$.each(states.data, function(){
			console.log(this);
			$("#stateSelect").append(
				$("<option></option>").text(this.state).val(this._id)
			);
		});
	});

});
