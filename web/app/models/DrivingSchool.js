// grab the mongoose module
var mongoose = require("mongoose");
<<<<<<< HEAD
var autoIncrement = require("mongoose-auto-increment");
autoIncrement.initialize(mongoose.connection);
=======
>>>>>>> d790af4220895f4f4691e797126914d853aeedf4
var Schema = mongoose.Schema;

// define our driving school model
// module.exports allows us to pass this to other files when it is called
var DrivingSchoolSchema = new Schema({
<<<<<<< HEAD
    schoolId: { type: Number, required: true, unique: true },
=======
    schoolId: { type: Number, required: true },
>>>>>>> d790af4220895f4f4691e797126914d853aeedf4
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
