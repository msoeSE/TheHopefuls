// grab the mongoose module
var mongoose = require("mongoose");

// define our driving school model
// module.exports allows us to pass this to other files when it is called
var DrivingSchoolSchema = new mongoose.Schema({
    schoolId: { type: Number, required: true },
    owners: [{ type: Schema.Types.ObjectId, ref: "User" }],
    instructors: [{ type: Schema.Types.ObjectId, ref: "User" }],
    students: [{ type: Schema.Types.ObjectId, ref: "User" }],
    address: { type: String, required: true },
    state: { type: String, required: true },
    zip: { type: String, required: true }
});

module.exports = mongoose.model("DrivingSchool", DrivingSchoolSchema);
