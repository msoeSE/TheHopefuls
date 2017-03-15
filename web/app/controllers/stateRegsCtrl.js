var StateRegulations = require("../models/StateRegulations");

exports.getStateRegs = function (schoolId, callback, error) {
	User.findOne({
		state: state
	}, function(err, doc) {
		if (err) {
			error({
				"message": "Error retrieving state regulations",
				"error": err
			});
			return;
		}
		callback(doc);
	});
};
