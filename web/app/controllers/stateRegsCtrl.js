var StateRegulations = require("../models/StateRegulations");

exports.getStateRegs = function (state, callback, error) {
	StateRegulations.findOne({
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
