var statusCodes = require("http-status-codes");
var UserCtrl = require(`${process.env.PWD}/app/controllers/userCtrl`);
var DrivingSchoolCtrl = require(`${process.env.PWD}/app/controllers/drivingSchoolCtrl`);
var DrivingSessionCtrl = require(`${process.env.PWD}/app/controllers/drivingSessionCtrl`);

// describe("Driving School Controller Tests", function() {
//     describe("Get Driving Sessions", function(){
//         beforeEach(function(){
//             spyOn(DrivingSessionCtrl, "listDrivingSessions").andCallFake(function(id, callback, failure){
//
//             });
//         });
//         it("should return valid response on correct controller call", function(){
//         });
//     });
// });
