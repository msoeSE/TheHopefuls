// grab the mongoose module
var mongoose = require("mongoose");
var Schema = mongoose.Schema;
<<<<<<< HEAD
var DrivingSession = require("../models/DrivingSession");
=======
>>>>>>> d790af4220895f4f4691e797126914d853aeedf4

// define our user model
// module.exports allows us to pass this to other files when it is called
var UserSchema = new Schema({
<<<<<<< HEAD
    loginDetails: {
      userId: String,
      service: String
    },
=======
    userId: { type: Number, required: true },
    email: { type: String, required: true },
>>>>>>> d790af4220895f4f4691e797126914d853aeedf4
    firstName: { type: String, required: true },
    lastName: { type: String, required: true },
    userType: { type: String, required: true },
    state: String,
    drivingSessions: [{ type: Schema.Types.ObjectId, ref: "DrivingSession" }]
});

module.exports = mongoose.model("User", UserSchema);
