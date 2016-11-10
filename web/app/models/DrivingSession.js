// grab the mongoose module
var mongoose = require("mongoose");

// define our driving session model
// module.exports allows us to pass this to other files when it is called
var DrivingSessionSchema = new mongoose.Schema({
    startTime: { type: Number, required: true },
    endTime: { type: Number, required: true },
    distance: { type: Number, required: true },
    duration: { type: Number, required: true },
    weatherData: [String]
});

module.exports = mongoose.model("DrivingSession", DrivingSessionSchema);
