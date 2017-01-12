var express = require("express");
var router = express.Router();

// frontend routes =========================================================
// route to handle all angular requests
var staticFilesLocation = process.env.NODE_ENV === "production" ? "dist" : "public";
router.get("/login", function(req, res) {
	if(req.isAuthenticated()) {
		res.redirect("/");
	} else {
		res.sendfile(`./${staticFilesLocation}/login.html`);
	}
});

// route to handle all angular requests
router.get("*", function(req, res) {
	if(req.isAuthenticated()) {
		res.sendfile(`./${staticFilesLocation}/index.html`);
	} else {
		res.redirect("/login");
	}
});
module.exports = router;
