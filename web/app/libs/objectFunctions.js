MissingProperties = function(object, properties) {
	if(typeof object !== "object" || !Array.isArray(properties)) // eslint-disable-line
		return;
	var missingProperties = [];
	properties.forEach((property) => {
		if (!object.hasOwnProperty(property))
			missingProperties.push(property);
	});
	return missingProperties;
};
