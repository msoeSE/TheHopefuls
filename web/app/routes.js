module.exports = function(app) {

	// server routes ===========================================================
	// handle things like api calls
	// authentication routes

	// TODO: add handling for bad api calls (i.e. invalid params, bad data formats)
  app.get("/api/students/:userId", function(req, res) {
		res.statusCode = 200;
		res.json({ message: "Successful request!" });
	});

	app.get("/api/students/:userId/drivingsessions", function(req, res) {
    res.statusCode = 200;
    res.json({});
	});

	app.get("/api/drivingschools", function(req, res) {
		res.statusCode = 200;
		res.json({});
	});

	app.get("/api/drivingschools/:schoolId", function(req, res) {
		res.statusCode = 200;
		res.json({});
	});

	app.get("/api/drivingschools/:schoolId/students", function(req, res) {
		res.statusCode = 200;
		res.json({});
	});

	app.get("/api/drivingschools/:schoolId/instructors", function(req, res) {
		res.statusCode = 200;
		res.json({});
	});

	// frontend routes =========================================================
	// route to handle all angular requests
	app.get('*', function(req, res) {
		res.sendfile('./public/index.html');
	});

};
