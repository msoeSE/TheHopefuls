// grab the mongoose module
var mongoose = require("mongoose");
var autoIncrement = require("mongoose-auto-increment");
autoIncrement.initialize(mongoose.connection);
var Schema = mongoose.Schema;

// define our user model
// module.exports allows us to pass this to other files when it is called
var UserSchema = new Schema({
    userId: { type: Number, required: true },
    email: { type: String, required: true },
    firstName: { type: String, required: true },
    lastName: { type: String, required: true },
    userType: { type: String, required: true },
    state: String,
    drivingSessions: [{ type: Schema.Types.ObjectId, ref: "DrivingSession" }]
});

UserSchema.plugin(autoIncrement.plugin, { model: "User", field: "userId", startAt: 4 });
module.exports = mongoose.model("User", UserSchema);
