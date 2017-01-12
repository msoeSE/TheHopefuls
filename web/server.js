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
var passportGoogleToken = require("passport-google-token");
var GoogleTokenStrategy = passportGoogleToken.Strategy;
var passportFacebookToken = require("passport-facebook-token");
var FacebookTokenStrategy = passportFacebookToken;
var requireDir = require("require-dir");
requireDir("./app/libs");
_ = require("underscore");

// configuration ===========================================

// If you don't have this file, contact Dylan for it.
var config = require("./config.json");

// config files
var db = require("./config/db");

var prodPort = config.ProdPort;
var defaultWebPort = 80;
var devPort = 3000;
var port = (process.env.NODE_ENV === "production" ? prodPort : devPort);

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
	app.use("/css", express.static(__dirname + "/dist/css"));
	app.use("/images", express.static(__dirname + "/dist/images"));
	app.use("/js", express.static(__dirname + "/dist/js"));
	app.use("/resources", express.static(__dirname + "/dist/resources"));
	app.use("/views", express.static(__dirname + "/dist/views"));

	passport.use(new GoogleStrategy({
		clientID:	 config.Auth.GoogleAuth.Production.ID,
		clientSecret: config.Auth.GoogleAuth.Production.Secret,
		callbackURL: `${config.Hostname}${port === defaultWebPort ? "" : (":" + port)}/auth/google/callback`,
		passReqToCallback: true
	},
	function(request, accessToken, refreshToken, profile, done) { // eslint-disable-line
		// User.findOrCreate({ googleId: profile.id }, function (err, user) {
		// 	return done(err, user);
		// });
		return done(null, profile);
	}));

	passport.use(new FacebookStrategy({
		clientID: config.Auth.FacebookAuth.Production.ID,
		clientSecret: config.Auth.FacebookAuth.Production.Secret,
		callbackURL: `${config.Hostname}${port === defaultWebPort ? "" : (":" + port)}/auth/facebook/callback`,
		profileFields: ["id", "email", "gender", "name"]
	},
	function(accessToken, refreshToken, profile, done) {
		// User.findOrCreate({ facebookId: profile.id }, function (err, user) {
		// 	return cb(err, user);
		// });
		return done(null, profile);
	}));

	passport.use(new GoogleTokenStrategy({
		clientID: config.Auth.GoogleAuth.Production.ID,
		clientSecret: config.Auth.GoogleAuth.Production.Secret
	},
	function(accessToken, refreshToken, profile, done) {
		// User.findOrCreate({ googleId: profile.id }, function (err, user) {
		// 	return done(err, user);
		// });
		return done(null, profile);
	}));

	passport.use(new FacebookTokenStrategy({
		clientID: config.Auth.FacebookAuth.Production.ID,
		clientSecret: config.Auth.FacebookAuth.Production.Secret
	}, function(accessToken, refreshToken, profile, done) {
		// User.findOrCreate({facebookId: profile.id}, function (error, user) {
		// 	return done(error, user);
		// });
		return done(null, profile);
	}));

} else {
	app.use("/css", express.static(__dirname + "/public/css"));
	app.use("/images", express.static(__dirname + "/public/images"));
	app.use("/js", express.static(__dirname + "/public/js"));
	app.use("/resources", express.static(__dirname + "/public/resources"));
	app.use("/views", express.static(__dirname + "/public/views"));

	passport.use(new GoogleStrategy({
		clientID:	 config.Auth.GoogleAuth.Local.ID,
		clientSecret: config.Auth.GoogleAuth.Local.Secret,
		callbackURL: `http://localhost${port === defaultWebPort ? "" : (":" + port)}/auth/google/callback`,
		passReqToCallback: true
	},
	function(request, accessToken, refreshToken, profile, done) { // eslint-disable-line
		// User.findOrCreate({ googleId: profile.id }, function (err, user) {
		// 	return done(err, user);
		// });
		return done(null, profile);
	}));

	passport.use(new FacebookStrategy({
		clientID: config.Auth.FacebookAuth.Local.ID,
		clientSecret: config.Auth.FacebookAuth.Local.Secret,
		callbackURL: `http://localhost${port === defaultWebPort ? "" : (":" + port)}/auth/facebook/callback`,
		profileFields: ["id", "email", "gender", "name"]
	},
	function(accessToken, refreshToken, profile, done) {
		// User.findOrCreate({ facebookId: profile.id }, function (err, user) {
		// 	return cb(err, user);
		// });
		return done(null, profile);
	}));

	passport.use(new GoogleTokenStrategy({
		clientID: config.Auth.GoogleAuth.Local.ID,
		clientSecret: config.Auth.GoogleAuth.Local.Secret
	},
	function(accessToken, refreshToken, profile, done) {
		// User.findOrCreate({ googleId: profile.id }, function (err, user) {
		// 	return done(err, user);
		// });
		return done(null, profile);
	}));

	passport.use(new FacebookTokenStrategy({
		clientID: config.Auth.FacebookAuth.Local.ID,
		clientSecret: config.Auth.FacebookAuth.Local.Secret
	}, function(accessToken, refreshToken, profile, done) {
		// User.findOrCreate({facebookId: profile.id}, function (error, user) {
		// 	return done(error, user);
		// });
		return done(null, profile);
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
app.post("/auth/google/token",
	passport.authenticate("google-token"),
	function(req, res) {
		res.send(req.user);
	}
);

app.post("/auth/facebook/token",
	passport.authenticate("facebook-token"),
	function (req, res) {
		// do something with req.user
		res.send(req.user);
	}
);

app.get("/profile",
	function(req, res){
		if(req.user)
			res.write(JSON.stringify(req.user)); // eslint-disable-line
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
// require("./app/routes")(app); // pass our application into our routes
var routes = require("./app/routes/mainRoutes");
app.use("/api", routes);

var index = require("./app/routes/index");
app.use("/", index);

// start app ===============================================
app.listen(port);
console.log("Magic happens on port " + port); // eslint-disable-line
exports = module.exports = app;
