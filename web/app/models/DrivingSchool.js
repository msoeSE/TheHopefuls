// grab the mongoose module
var mongoose = require("mongoose");
var User = require("../models/User");

// define our driving school model
// module.exports allows us to pass this to other files when it is called
module.exports = mongoose.model("User",{
    schoolId: { type: Number, required: true },
    owners: [User],
    instructors: [User],
    students: [User],
    address: { type: String, required: true },
    state: { type: String, required: true },
    zip: { type: String, required: true }
});
