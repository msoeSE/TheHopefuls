/*eslint-disable */
var gulp = require("gulp");
var eslint = require("gulp-eslint");
var uglify = require("gulp-uglify");
var cleanCSS = require("gulp-clean-css");
var imagemin = require("gulp-imagemin");
var zopfli = require("imagemin-zopfli");
var htmlmin = require("gulp-htmlmin");
var ngAnnotate = require("gulp-ng-annotate");
var pump = require("pump");
var server = require('karma').Karma;

gulp.task("lint", function() {
	return gulp.src(["app/**/*.js", "public/js/**/*.js"])
		.pipe(eslint())
		.pipe(eslint.format())
		.pipe(eslint.failAfterError());
});

gulp.task("minify-js", function (cb) {
  pump([
		gulp.src("public/js/**/*.js"),
		ngAnnotate(),
		uglify(),
		gulp.dest("dist/js/")
	],
	cb
  );
});

gulp.task("minify-html", function() {
  return gulp.src("public/**/*.html")
	.pipe(htmlmin({collapseWhitespace: true}))
	.pipe(gulp.dest("dist/"));
});

gulp.task("minify-css", function() {
  return gulp.src("public/css/**/*.css")
	.pipe(cleanCSS({compatibility: "ie8"}))
	.pipe(gulp.dest("dist/css/"));
});

gulp.task("minify-img", function() {
	return gulp.src("public/images/*")
		.pipe(imagemin({
			use: [zopfli()]
		}))
		.pipe(gulp.dest("dist/images/"));
});

gulp.task("build",
	[
		"minify-html",
		"minify-css",
		"minify-js",
		"minify-img"
	]
);

gulp.task("default",
	[
		"lint"
		// "compress",
		// "minify-css"
	]
);
