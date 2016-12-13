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
	var staticFilesLocation = process.env.NODE_ENV === "production" ? "dist" : "public";
	console.log(staticFilesLocation);
	console.log(`./${staticFilesLocation}/login.html`);
	// frontend routes =========================================================
	app.get("/login", function(req, res) {
		if(req.isAuthenticated()) {
			res.redirect("/");
		} else {
			res.sendfile(`./${staticFilesLocation}/login.html`);
		}
	});

	// route to handle all angular requests
	app.get("*", function(req, res) {
		if(req.isAuthenticated()) {
			res.sendfile(`./${staticFilesLocation}/index.html`);
		} else {
			res.redirect("/login");
		}
	});
};
