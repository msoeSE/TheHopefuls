/* eslint angular/log: 0 angular/json-functions: 0*/

// Mongo files
var userCtrl = require("./app/controllers/userCtrl");

// modules =================================================
var express		= require("express");
var app			= express();
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
var mongoose = require("mongoose");
var stateRegs = require("./app/models/StateRegulations");
var fs = require("fs");

// configuration ===========================================
// If you don't have this file, contact Dylan for it.
var config = require("./config.json");
var port = config.Port;


// TODO Replace with config
mongoose.connect("mongodb://localhost/routerdb");

console.log("Attempting to load state regulations");
fs.access("./stateregs.json", fs.constants.R_OK, (err) => {
	console.log(err ? "Cannot load state regulations file" : "State regulations file loaded");
	if(err)
		return;
	var regs = require("./stateregs.json"); // eslint-disable-line
	regs.forEach((reg)=>{
		stateRegs.findOneAndUpdate({state: reg.state}, reg, {upsert: true}, (err2)=>{
			if(err2)
				console.log(err2);
		});
	});
});

app.use(bodyParser.json());
app.use(bodyParser.json({ type: "application/vnd.api+json" }));
app.use(bodyParser.urlencoded({ extended: true }));
app.use(methodOverride("X-HTTP-Method-Override"));
app.use(session({
	secret: config.Auth.Secret,
	name: "session-id",
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

passport.use(
	new GoogleStrategy({
		clientID:	 config.Auth.GoogleAuth.ID,
		clientSecret: config.Auth.GoogleAuth.Secret,
		callbackURL: `${config.Auth.CallbackURLBase}/auth/google/callback`,
		passReqToCallback: true
	}, function(request, accessToken, refreshToken, profile, done) { // eslint-disable-line
		// User.findOrCreate({ googleId: profile.id }, function (err, user) {
		// 	return done(err, user);
		// });
		return done(null, profile);
	}
));

passport.use(
	new FacebookStrategy({
		clientID: config.Auth.FacebookAuth.ID,
		clientSecret: config.Auth.FacebookAuth.Secret,
		callbackURL: `${config.Auth.CallbackURLBase}/auth/facebook/callback`,
		profileFields: ["id", "email", "gender", "name", "picture.type(large)"]
	}, function(accessToken, refreshToken, profile, done) {
		getOrCreateUser(profile, done);
		// return done(null, profile);
	}
));

function getOrCreateUser(profile, done) {
	userCtrl.getUser(profile.id, function(doc){
		if(!doc){
			var json = {
				firstName: profile._json.first_name,
				lastName: profile._json.last_name,
				userId: profile.id,
				service: "facebook"
			};

			userCtrl.createUser(json, "student", function(user) {
				return done(null, profile);
			}, function(error) {
				return done(error);
				$log.log(error);
			});
		} else {
			return done(null, profile);
		}
	}, function(error){
		return done(error);
		console.log(error);
	});
}

passport.use(
	new GoogleTokenStrategy({
		clientID: config.Auth.GoogleAuth.ID,
		clientSecret: config.Auth.GoogleAuth.Secret
	}, function(accessToken, refreshToken, profile, done) {
		// User.findOrCreate({ googleId: profile.id }, function (err, user) {
		// 	return done(err, user);
		// });
		return done(null, profile);
	}
));

passport.use(
	new FacebookTokenStrategy({
		clientID: config.Auth.FacebookAuth.ID,
		clientSecret: config.Auth.FacebookAuth.Secret
	}, function(accessToken, refreshToken, profile, done) {
		getOrCreateUser(profile, done);
		// return done(null, profile);
	}
));

var staticFilesDir = __dirname + (process.env.NODE_ENV === "production" ? "/dist" : "/public");
app.use("/css", express.static(staticFilesDir + "/css"));
app.use("/images", express.static(staticFilesDir + "/images"));
app.use("/js", express.static(staticFilesDir + "/js"));
app.use("/resources", express.static(staticFilesDir + "/resources"));
app.use("/views", express.static(staticFilesDir + "/views"));
app.use("/fonts", express.static(staticFilesDir + "/fonts"));
app.use("/favicon.ico", express.static(staticFilesDir + "/favicon.ico"));


app.get("/auth/facebook", passport.authenticate("facebook"));
app.get("/auth/facebook/callback",
	passport.authenticate("facebook", { failureRedirect: "/login" }),
	function(req, res) {// Rudimentary way of updating req.user
		userCtrl.getUser(req.user.id, function(doc){
			if(doc){
				req.user.userType = doc.userType;
				req.user.mongoID = doc._id;
				req.user.schoolId = doc.schoolId;
			}
			res.redirect("/");
		}, function (err){
			console.log(err);
			res.redirect("/");
		});
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
	function (req, res) {// Rudimentary way of updating req.user
		userCtrl.getUser(req.user.id, function(doc){
			if(doc){
				req.user.userType = doc.userType;
				req.user.mongoID = doc._id;
				req.user.schoolId = doc.schoolId;
			}
			res.send(req.user);
		}, function (err){
			console.log(err);
			res.send(req.user);
		});
	}
);

app.get("/profile",
	function(req, res){
		if(req.user){
			userCtrl.getUser(req.user.id, function(doc){
				if(doc){
					req.user.userType = doc.userType;
					req.user.mongoID = doc._id;
					console.log(doc.schoolId);
					req.user.schoolId = doc.schoolId;
				}
				res.send(JSON.stringify(req.user));
			}, function (err){
				console.log(err);
				res.end();
			});
		} else {
			res.redirect("/");
		}
	}
);
app.get("/auth/logout",
	function(req, res){
		req.logout();
		res.redirect("/");
	}
);

// routes ==================================================
var routes = require("./app/routes/mainRoutes");
app.use("/api", routes);

var index = require("./app/routes/index");
app.use("/", index);

// start app ===============================================
app.listen(port);
console.log("Magic happens on port " + port);
exports = module.exports = app;
