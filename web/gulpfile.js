/*eslint-disable */
var gulp = require("gulp");
var eslint = require("gulp-eslint");
var uglify = require('gulp-uglify');
var cleanCSS = require('gulp-clean-css');
var pump = require('pump');

gulp.task("lint", function() {
    return gulp.src(["app/**/*.js", "public/js/**/*.js"])
        // eslint() attaches the lint output to the "eslint" property
        // of the file object so it can be used by other modules.
        .pipe(eslint())
        // eslint.format() outputs the lint results to the console.
        // Alternatively use eslint.formatEach() (see Docs).
        .pipe(eslint.format())
        // To have the process exit with an error code (1) on
        // lint error, return the stream and pipe to failAfterError last.
        .pipe(eslint.failAfterError());
});

gulp.task('compress', function (cb) {
  pump([
        gulp.src('public/js/**/*.js'),
        uglify(),
        gulp.dest('dist/js/')
    ],
    cb
  );
});

gulp.task('minify-css', function() {
  return gulp.src('public/css/**/*.css')
    .pipe(cleanCSS({compatibility: 'ie8'}))
    .pipe(gulp.dest('dist/css/'));
});

gulp.task('default',
    [
    'lint'
    // 'compress',
    // 'minify-css'
    ]
);
