// grab the mongoose module
var mongoose = require("mongoose");
var Schema = mongoose.Schema;

// define our user model
// module.exports allows us to pass this to other files when it is called
var UserSchema = new Schema({
	loginDetails: {
		userId: String,
		service: String
	},
	firstName: {
		type: String,
		required: true
	},
	lastName: {
		type: String,
		required: true
	},
	userType: {
		type: String,
		required: true
	},
	state: String,
	schoolId: Number,
	drivingSessions: [{
		type: Schema.Types.Object,
		ref: "DrivingSession"
	}]
});

module.exports = mongoose.model("User", UserSchema);
