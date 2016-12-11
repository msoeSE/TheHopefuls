var DrivingSession = require("../models/DrivingSession");
var User = require("../models/User");

//TODO: find an accurate formula for distance from gps points
function calculateDistance(drivePoints) {
  var distance;
  return distance;
}

exports.createDrivingSession = function(req, res) {
  var finalDistance;
  var newDrivingSession = new DrivingSession({
    startTime: req.body.startTime,
    endTime: req.body.endTime,
    distance: finalDistance,
    //TODO: figure out what units are
    duration: req.body.duration,
    weatherData: [String]
  });

  if(!req.body.startTime) {
    res.status(400);
    res.send("Start time required!");
  } else if(!req.body.endTime){
    res.status(400);
    res.send("End time required!");
  } else if(!req.body.duration){
    res.status(400);
    res.send("Duration required!");
  } else {
    newDrivingSession.save(function(err) {
      if(!err) {
        res.status(201);
        res.json(newDrivingSession);
      } else {
        json.send("Error saving driving session, " + err);
      }
    });
  }

  User.find({ _id: req.params._id }, function(err, student) {
		if(!err) {
      student.drivingSessions.push(newDrivingSession);
      student.save(function(err) {
        if(err){
          res.send("Error saving driving sessioin to student, " + err);
        }
      });
			res.json(student);
		} else {
			res.send("Error retreiving driving session data, " + err);
		}
  });
};

exports.listDrivingSessions = function(req, res) {
  User.find({ _id: req.params._id }, function(err, student) {
		if(!err) {
			res.statusCode = 200;
			res.json(student.drivingSessions);
		} else {
			res.send("Error retreiving driving session data, " + err);
		}
  });
};
