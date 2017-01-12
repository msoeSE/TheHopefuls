Object.prototype.missingProperties = function(properties) {
	if(!Array.isArray(properties)) // eslint-disable-line
		return;
	var missingProperties = [];
	properties.forEach((property) => {
		if (!this.hasOwnProperty(property))
			missingProperties.push(property);
	});
	return missingProperties;
};
