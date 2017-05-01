// grab the mongoose module
var mongoose = require("mongoose");
var Schema = mongoose.Schema;

// define our driving session model
// module.exports allows us to pass this to other files when it is called
var StateRegulationSchema = new Schema({
	state: {
		type: String,
		required: true
	},
	stateAbbreviation: {
		type: String,
		required: true
	},
	noHoursWithDriversEdHours: {
		type: Boolean,
		required: true
	},
	dayHours: {
		type: Number,
		required: true
	},
	nightHours: {
		type: Number,
		required: true
	},
	inclementWeather: {
		type: Number,
		required: true
	},
	nightOrInclement: {
		type: Boolean,
		required: true
	},
	notes: {
		type: String,
		required: false
	}
});

module.exports = mongoose.model("StateRegulation", StateRegulationSchema);
