module.exports = function(app) {

	// server routes ===========================================================
	// handle things like api calls
	// authentication routes

	// frontend routes =========================================================
	var staticFilesLocation = process.env.NODE_ENV === "production" ? "dist" : "public";

	app.get("/login-success", function(req, res){
		res.sendfile(`./${staticFilesLocation}/login-success.html`);
	});

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
