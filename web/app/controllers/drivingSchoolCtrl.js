var DrivingSchool = require("../models/DrivingSchool");

exports.createSchool = function(req, res) {
  var newSchool = new DrivingSchool({
    addressLine1: req.body.addressLine1,
    addressLine2: req.body.addressLine2,
    state: req.body.state,
    zip: req.body.zip
  });

  if(!req.body.addressLine1) {
    res.status(400);
    res.send("Adress required!");
  } else if(!req.body.state) {
    res.status(400);
    res.send("State required!");
  } else if(!req.body.zip) {
    res.status(400);
    res.send("Zipcode required!");
  }
  else {
    newSchool.save(function(err) {
      if(!err) {
        res.statusCode = 201;
        res.json(newSchool);
      } else {
        res.send("Error creating a new school, " + err);
      }
    });
  }
};

exports.getSchool = function(req, res) {
  DrivingSchool.find({ schoolId: req.params.schoolId }, function(err, doc) {
		if(!err) {
			res.statusCode = 200;
			res.json(doc);
		} else {
			res.send("Error retreiving student's data, " + err);
		}
  });
};
