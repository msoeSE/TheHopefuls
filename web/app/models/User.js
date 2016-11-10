// grab the mongoose module
var mongoose = require("mongoose");

// define our user model
// module.exports allows us to pass this to other files when it is called

var UserSchema = new mongoose.Schema({
    userId: { type: Number, required: true },
    email: { type: String, required: true },
    firstName: { type: String, required: true },
    lastName: { type: String, required: true },
    password: { type: String, required: true },
    userType: { type: String, required: true },
    state: String,
    drivingSessions:
    [
        {
            startTime: { type: Number, required: true },
            endTime: { type: Number, required: true },
            distance: { type: Number, required: true },
            duration: { type: Number, required: true },
            weatherData: [String]
        }
    ]
});

module.exports = mongoose.model("User", UserSchema);
