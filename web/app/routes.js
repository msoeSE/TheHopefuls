module.exports = function(app) {

	// server routes ===========================================================
	// handle things like api calls
	// authentication routes
	function ensureAuthenticated(req, res, next){
		if(req.isAuthenticated()){
			next();
		} else {
			let forbidden = 403;
			res.status(forbidden);
			res.json({error: "Not authenticated"});
		}
	}

	app.get("/api/test", ensureAuthenticated, function(req, res){
		res.send("Test");
	});

	// frontend routes =========================================================
	app.get("/login", function(req, res) {
		console.log("Login");
		if(req.isAuthenticated()) {
			console.log("Authenticated");
			res.redirect("/");
		} else {
			console.log("Not Authenticated");
			res.sendfile("./public/login.html");
		}
	});

	// route to handle all angular requests
	app.get("*", function(req, res) {
		console.log("Home");
		if(req.isAuthenticated()) {
			console.log("Authenticated");
			res.sendfile("./public/index.html");
		} else {
			console.log("Not Authenticated");
			res.redirect("/login");
		}
	});
};
