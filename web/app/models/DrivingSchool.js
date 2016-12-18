// grab the mongoose module
var mongoose = require("mongoose");
var Schema = mongoose.Schema;
var autoIncrement = require("mongoose-auto-increment");
autoIncrement.initialize(mongoose.connection);

// define our driving school model
// module.exports allows us to pass this to other files when it is called
var DrivingSchoolSchema = new Schema({
    schoolId: { type: Number, required: true, unique: true },
    owners: [{ type: Schema.Types.ObjectId, ref: "User" }],
    instructors: [{ type: Schema.Types.ObjectId, ref: "User" }],
    students: [{ type: Schema.Types.ObjectId, ref: "User" }],
    addressLine1: { type: String, required: true },
    addressLine2: String,
    state: { type: String, required: true },
    zip: { type: Number, required: true }
});

DrivingSchoolSchema.plugin(autoIncrement.plugin, { model: "DrivingSchool", field: "schoolId", startAt: 0 });
module.exports = mongoose.model("DrivingSchool", DrivingSchoolSchema);
