// modules =================================================
var express		= require("express");
var app			= express();
var mongoose	   = require("mongoose");
var bodyParser	 = require("body-parser");
var methodOverride = require("method-override");
var session = require("express-session");
var passport = require("passport");
var passportGoogle = require("passport-google-oauth2");
var GoogleStrategy = passportGoogle.Strategy;
var passportFacebook = require("passport-facebook");
var FacebookStrategy = passportFacebook.Strategy;


// configuration ===========================================

// config files
var db = require("./config/db");
var prodPort = 80;
var devPort = 3000;
var port = (process.env.NODE_ENV === "production" ? prodPort : devPort);

// If you don't have this file, contact Dylan for it.
var config = require("./config.json");

// mongoose.connect(db.url); // connect to our mongoDB database (commented out after you enter in your own credentials)

app.use(bodyParser.json());
app.use(bodyParser.json({ type: "application/vnd.api+json" }));
app.use(bodyParser.urlencoded({ extended: true }));
app.use(methodOverride("X-HTTP-Method-Override"));
app.use(session({
	secret: config.Auth.Secret,
	name: "sessId",
	resave: true,
	saveUninitialized: true}));
app.use(passport.initialize());
app.use(passport.session());
passport.serializeUser(function(user, done) {
	done(null, user);
});
passport.deserializeUser(function(obj, done) {
	done(null, obj);
});

if(process.env.NODE_ENV === "production") {
	app.use(express.static(__dirname + "/dist"));

} else {
	app.use(express.static(__dirname + "/public"));

	passport.use(new GoogleStrategy({
		clientID:	 config.Auth.GoogleAuth.Local.ID,
		clientSecret: config.Auth.GoogleAuth.Local.Secret,
		callbackURL: `http://localhost:${port}/auth/google/callback`,
		passReqToCallback: true
	},
	function(request, accessToken, refreshToken, profile, done) {
		// User.findOrCreate({ googleId: profile.id }, function (err, user) {
		// 	return done(err, user);
		// });
		return done(null, profile);
	}));

	passport.use(new FacebookStrategy({
		clientID: config.Auth.FacebookAuth.Local.ID,
		clientSecret: config.Auth.FacebookAuth.Local.Secret,
		callbackURL: `http://localhost:${port}/auth/facebook/callback`,
		profileFields: ["id", "email", "gender", "name"]
	},
	function(accessToken, refreshToken, profile, cb) {
		// User.findOrCreate({ facebookId: profile.id }, function (err, user) {
		// 	return cb(err, user);
		// });
		return cb(null, profile);
	}));
}
app.use("/libs", express.static(__dirname + "/libs"));

app.get("/auth/facebook", passport.authenticate("facebook"));
app.get("/auth/facebook/callback",
	passport.authenticate("facebook", { failureRedirect: "/login" }),
	function(req, res) {
		res.redirect("/");
	}
);
app.get("/auth/google", passport.authenticate("google", { scope:
	["https://www.googleapis.com/auth/userinfo.profile"]
}));
app.get("/auth/google/callback",
	passport.authenticate("google", { failureRedirect: "/login" }),
	function(req, res) {
		res.redirect("/");
	}
);
app.get("/profile",
	function(req, res){
		if(req.user)
			res.write(JSON.stringify(req.user));
		res.end();
	}
);
app.get("/auth/logout",
	function(req, res){
		req.logout();
		res.redirect("/");
	}
);
// routes ==================================================
require("./app/routes")(app); // pass our application into our routes

// start app ===============================================
app.listen(port);
console.log("Magic happens on port " + port); // eslint-disable-line
exports = module.exports = app;
