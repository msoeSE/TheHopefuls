var StateRegulations = require("../models/StateRegulations");

exports.getStateRegs = function(state, callback, error){
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

exports.getStates = function(callback, error){
	StateRegulations.find({}, {state: 1, $orderby: "state" }, (err, states)=>{
		if(err){
			error({
				"message": "Error retrieving states list",
				"error": err
			});
			return;
		}
		callback(states);
	});
};
