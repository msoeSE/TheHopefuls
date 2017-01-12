var express = require("express");
var router = express.Router();

// frontend routes =========================================================
// route to handle all angular requests
router.get("*", function(req, res) {
	res.sendfile("./public/index.html");
});

module.exports = router;
