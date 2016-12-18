var User = require("../models/User");

exports.createStudent = function(req, res) {
  var newStudent = new User({
    loginDetails: {
      userId: req.body.loginDetails.userId,
      service: req.body.loginDetails.service
    },
    firstName: req.body.firstName,
    lastName: req.body.lastName,
    userType: "student"
  });

  if(!req.body.firstName) {
    res.status(400);
    res.send("First name required!");
  } else if(!req.body.lastName) {
    res.status(400);
    res.send("Last name required!");
  } else {
    newStudent.save(function(err) {
      if(!err) {
        res.statusCode = 201;
        res.json(newStudent);
      } else {
        res.send("Error creating a new student, " + err);
      }
    });
  }
};

exports.getStudent = function(req, res) {
  User.findOne({ _id: req.params._id }, function(err, doc) {
		if(!err) {
			res.statusCode = 200;
			res.json(doc);
		} else {
			res.send("Error retreiving student's data, " + err);
		}
  });
};

exports.createInstructor = function(req, res) {
  var newInstructor = new User({
    loginDetails: {
      userId: req.body.loginDetails.userId,
      service: req.body.loginDetails.service
    },
    firstName: req.body.firstName,
    lastName: req.body.lastName,
    userType: "instructor"
  });

  if(!req.body.firstName) {
    res.status(400);
    res.send("First name required!");
  } else if(!req.body.lastName) {
    res.status(400);
    res.send("Last name required!");
  } else {
    newInstructor.save(function(err) {
      if(!err) {
        res.statusCode = 201;
        res.json(newInstructor);
      } else {
        res.send("Error creating a new instructor, " + err);
      }
    });
  }
};

exports.getInstructor = function(req, res) {
  User.findOne({ _id: req.params._id }, function(err, doc) {
		if(!err) {
			res.statusCode = 200;
			res.json(doc);
		} else {
			res.send("Error retreiving instructor's data, " + err);
		}
  });
};
