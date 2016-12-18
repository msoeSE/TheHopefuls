var DrivingSession = require("../models/DrivingSession");
var User = require("../models/User");

//TODO: find an accurate formula for distance from gps points
function calculateDistance(drivePoints) {
  // 0 for now
  var distance = 0;
  // drivePoints obj has lat, lon, speed, time
  return distance;
}

function calculateDuration(startTime, endTime) {
  // Calculate duration from times in date
  var duration = 0;
  return duration;
}

function updateStudentDrive(userId, newDrivingSession, res) {
  User.findOne({ _id: userId }, function(err, student) {
    if(!err) {
      student.drivingSessions.push(newDrivingSession);
      student.save(function(saveErr) {
        if(saveErr){
          res.send("Error saving driving sessioin to student, " + saveErr);
        }
      });
    } else {
      res.send("Error retreiving driving session data, " + err);
    }
  });
}

function checkCreateRequest(newDrivingSession, res) {
  if(!newDrivingSession.startTime) {
    res.status(400);
    res.send("Start time required!");
  } else if(!newDrivingSession.endTime){
    res.status(400);
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
}

exports.createDrivingSession = function(req, res) {
  // console.log(req.body);
  driveSessions = req.body;
  driveSessions.forEach(function(driveSession) {
    var finalDistance = calculateDistance(driveSession.drivePoints);
    var finalDuration = calculateDuration(driveSession.unsyncDrive.startTime,
                                            driveSession.unsyncDrive.endTime);
   newDrivingSession = new DrivingSession({
      startTime: new Date(driveSession.unsyncDrive.startTime),
      endTime: new Date(driveSession.unsyncDrive.endTime),
      distance: finalDistance,
      //TODO: figure out what units are
      duration: finalDuration,
      weatherData: {
        temperature: driveSession.weatherData.temperature,
        summary: driveSession.weatherData.summary
      }
    });

    checkCreateRequest(newDrivingSession, res);
    updateStudentDrive(req.params.userId, newDrivingSession, res);
  });
};

exports.listDrivingSessions = function(req, res) {
  User.findOne({_id: req.params.userId}, "drivingSessions")
    .populate("drivingSessions").exec(function(err, drivingSessions){
      if(!err) {
        res.statusCode = 200;
        res.json(drivingSessions);
      } else {
        res.send("Error retreiving driving session data, " + err);
      }
    });
};
