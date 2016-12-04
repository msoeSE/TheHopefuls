// grab the mongoose module
var mongoose = require("mongoose");
var Schema = mongoose.Schema;

// define our driving school model
// module.exports allows us to pass this to other files when it is called
var DrivingSchoolSchema = new Schema({
    schoolId: { type: Number, required: true },
    owners: [{ type: Schema.Types.ObjectId, ref: "User" }],
    instructors: [{ type: Schema.Types.ObjectId, ref: "User" }],
    students: [{ type: Schema.Types.ObjectId, ref: "User" }],
    addressLine1: { type: String, required: true },
    addressLine2: { type: String, required: true },
    state: { type: String, required: true },
    zip: { type: Number, required: true }
});

module.exports = mongoose.model("DrivingSchool", DrivingSchoolSchema);
