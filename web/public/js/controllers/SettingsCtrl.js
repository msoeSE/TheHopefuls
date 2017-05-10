angular.module("SettingsCtrl", ["SettingsService"]).controller("SettingsController", function($log, SettingsOb) {
	var vm = this;

	SettingsOb.state = "Wisconsin";

	SettingsOb.getProfile().then(function(settings){
		vm.username = settings._json.first_name + " " + settings._json.last_name;
		vm.profilePicture = settings._json.picture.data.url;
		vm.instructorID = settings.mongoID;
		if(settings.schoolId !== undefined){//eslint-disable-line
			vm.schoolId = settings.schoolId;
		}
	});

	$("#stateSelect").on("change", function(event){ //eslint-disable-line
    SettingsOb.state = $("#stateSelect :selected").text();
  });

	SettingsOb.getStates().then(function(states){
		$.each(states.data, function(){
			$("#stateSelect").append(
				$("<option></option>").text(this.state).val(this._id)//eslint-disable-line
			);
		});
	});

	$("#saveBtn").on("click", function (){
		var schoolId = $("#school_id").val();
		SettingsOb.getProfile().then(function (profile){
			SettingsOb.updateSchoolCode(schoolId, profile.id);
		});
	});
});
