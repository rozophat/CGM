var gulp = require('gulp');

var jshint = require('gulp-jshint');
var jshintreporter = require('jshint-stylish');
//var sass = require('gulp-sass');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var size = require('gulp-size');
var clean = require('gulp-clean');
var util = require('gulp-util');
//var ngmin = require('gulp-ngmin');      // Minify AngularJS Modules and Controller
var ngAnnotate = require('gulp-ng-annotate');      // Minify AngularJS Modules and Controller (better than ngmin)
var tap = require('gulp-tap');          // To detect file name is being process
var sequence = require('run-sequence');
var minifycss = require('gulp-minify-css');

// Declare global file path
var filePath = {
	appjs: { src: ['./app/**/*.js', '!/**/libs/**', '!/**/_references.js', '!/**/config.js'], dest: './dist' },
	//appjs: { src: ['./app/**/*.js', '!/**/_references.js', '!/**/config.js', , '!*.min.js', '!/**/*.min.js', '!/**/i18n/**'], dest: './dist' },
	configjs: { src: ['./app/config.js'], dest: './dist' },
	libsjs: { src: ['./app/scripts/libs/**/*.js', '!*.min.js', '!/**/*.min.js', '!/**/i18n/**'], dest: './dist/libs/' },
	jshint: { src: ['./app/**/*.js', '!/**/libs/**', '!/**/_references.js'] },
	css: { src: ['./app/styles/**/*.css', '!*.min.css', '!*.scss', '!/**/*.min.css'], dest: './dist/styles/' },
	cssimage: { src: ['./app/styles/**/*.*', '!*.min.css', '!*.map', '!*.css', '!/**/*.css', '!/**/*.min.css'], dest: './dist/styles/' },
	view: { src: './app/views/**/*', dest: './dist/views/' },
};

// Check Js grammar
gulp.task('jshint', function () {
	util.log(util.colors.yellow('Detecting all your javascript synstax error...'));
	gulp.src(filePath.jshint.src)
		 .pipe(jshint())
		 .pipe(jshint.reporter(jshintreporter));

	util.log(util.colors.yellow("Everything OK !!!You're doing goodjob!"));
});

// Concatenate & Minify App JS
gulp.task('app-minify', function () {
	util.log(util.colors.yellow('[JAVASCRIPT] Copy all applicaiton javarscript files to /dist/app.min.js'));

	gulp.src(filePath.appjs.src)
		 .pipe(concat('app.js'))
		 .pipe(rename('app.min.js'))
		 .pipe(ngAnnotate())
		 .pipe(uglify())
		 .pipe(size({ title: 'Total application javascript file size:', showFiles: false }))
		 .pipe(gulp.dest(filePath.appjs.dest));

	gulp.src(filePath.configjs.src)
		 .pipe(gulp.dest(filePath.configjs.dest));
});

gulp.task('app-copy', function () {
	util.log(util.colors.yellow('[JAVASCRIPT] Copy all application javarscript files to /dist/app.min.js'));

	gulp.src(filePath.appjs.src)
		 .pipe(concat('app.js'))
		 .pipe(rename('app.min.js'))
		 .pipe(size({ title: 'Total application javascript file size:', showFiles: false }))
		 .pipe(gulp.dest(filePath.appjs.dest));

	gulp.src(filePath.configjs.src)
		 .pipe(gulp.dest(filePath.configjs.dest));
});

// Concatenate & Minify Library JS
gulp.task('lib-minify', function () {
	util.log(util.colors.yellow('[JAVASCRIPT] Copy all javarscript libs to /dist/libs/'));

	gulp.src(filePath.libsjs.src)
		 .pipe(ngAnnotate())
		 .pipe(uglify())
		 .pipe(rename({ suffix: '.min' }))
		 .pipe(size({ title: "Total javascript libs file size: ", showFiles: false }))
		 .pipe(gulp.dest(filePath.libsjs.dest));
});

gulp.task('lib-copy', function () {
	util.log(util.colors.yellow('[JAVASCRIPT] Copy all javarscript libs to /dist/libs/'));

	gulp.src(filePath.libsjs.src)
		 .pipe(rename({ suffix: '.min' }))
		 .pipe(size({ title: "Total javascript libs file size: ", showFiles: false }))
		 .pipe(gulp.dest(filePath.libsjs.dest));
});

//Styles task
gulp.task('css-minify', function () {
	util.log(util.colors.yellow('[STYLES] Copy all css files to /dist/styles/'));

	gulp.src(filePath.css.src)
		 .pipe(minifycss())
		 .pipe(rename({ suffix: '.min' }))
		 .pipe(size({ title: "Total css file size: ", showFiles: false }))
		 .pipe(gulp.dest(filePath.css.dest));

	gulp.src(filePath.cssimage.src)
		 .pipe(size({ title: "Total cssimage file size: ", showFiles: false }))
		 .pipe(gulp.dest(filePath.cssimage.dest));
});

gulp.task('css-copy', function () {
	util.log(util.colors.yellow('[STYLES] Copy all css files to /dist/styles/'));

	gulp.src(filePath.css.src)
		 .pipe(rename({ suffix: '.min' }))
		 .pipe(size({ title: "Total css file size: ", showFiles: false }))
		 .pipe(gulp.dest(filePath.css.dest));

	gulp.src(filePath.cssimage.src)
		 .pipe(size({ title: "Total cssimage file size: ", showFiles: false }))
		 .pipe(gulp.dest(filePath.cssimage.dest));
});

gulp.task('clean', function () {
	gulp.src(
		 ['./dist/*'], { read: false })

	.pipe(clean({ force: true }));
});

// Views task
gulp.task('views', function () {
	util.log(util.colors.yellow('[FONTS] Copy all font files to /dist/'));
	gulp.src('./app/fonts/**/*.*')
		 .pipe(size({ title: "Total font file size: ", showFiles: false }))
		 .pipe(gulp.dest('dist/fonts/'));

	util.log(util.colors.yellow('[IMAGES] Copy all images files to /dist/'));
	gulp.src('./app/images/**/*.*')
		 .pipe(size({ title: "Total images file size: ", showFiles: false }))
		 .pipe(gulp.dest('dist/images/'));

	util.log(util.colors.yellow('[ASSETS] Copy all asset files to /dist/'));
	gulp.src('./app/assets/**/*.*')
		 .pipe(size({ title: "Total assets file size: ", showFiles: false }))
		 .pipe(gulp.dest('dist/assets/'));

	util.log(util.colors.yellow('[VIEWS] Copy all html files to /dist/views/'));
	// Any other view files from app/views
	gulp.src(filePath.view.src)
	// Will be put in the dist/views folder
		 .pipe(size({ title: "Total html file size: ", showFiles: false }))
		 .pipe(gulp.dest(filePath.view.dest));
});


// Watch Files For Changes
gulp.task('watch-release', function () {
	gulp.watch('./app/**/*.js', ['jshint', 'app-minify', 'lib-minify']);
	gulp.watch(filePath.view.src, ['views']);
	gulp.watch('./app/styles/**/*.css', ['css-minify']);

	util.log(util.colors.red("Waiting for your changes..."));
	util.beep();
});

// Watch files for develop
gulp.task('watch-dev', function () {
	gulp.watch('./app/**/*.js', ['jshint', 'app-copy', 'lib-copy']);
	gulp.watch(filePath.view.src, ['views']);
	gulp.watch('./app/styles/**/*.css', ['css-copy']);

	util.log(util.colors.red("Waiting for your changes..."));
	util.beep();
});

// Full build for dev - clean build folder, build js, build html
gulp.task('build-dev', function (callback) {
	sequence(
			'clean',
			'jshint',
			'app-copy',
			'lib-copy',
			'views',
			'css-copy',
			'watch-dev',
			callback
	  );
});

// Full build for release - clean build folder, build js, build html
gulp.task('build-release', function (callback) {
	sequence(
			'clean',
			'jshint',
			'app-minify',
			'lib-minify',
			'views',
			'css-minify',
			'watch-release',
			callback
	  );
});

//Tasks
gulp.task('release', ['build-release']),
gulp.task('dev', ['build-dev']);
