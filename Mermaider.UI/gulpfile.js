/// <binding AfterBuild='clean, repack-bootstrap, move-content-to-wwwroot, browserify-mermaid' Clean='clean' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

// Gulp Dependencies
var gulp = require('gulp');
var debug = require('gulp-debug');
var del = require('del');
var rename = require('gulp-rename');
var gless = require('gulp-less');

// Build Dependencies
var browserify = require('gulp-browserify');
var uglify = require('gulp-uglify');

// Dev dependencies
var jshint = require('gulp-jshint');

// Common Setups - this should be copying the site.css too.
var sourcePaths = {
    typeScriptStuff: ['scripts/**/*.ts', 'scripts/**/*.map'],
    scriptStuff: ['scripts/**/*.js', 'bower_components/jquery/dist/*.*'],
    allScriptStuff: [],
    cssFiles: ['node_modules/mermaid/dist/*.css', 'styles/*.*']
};
sourcePaths.allScriptStuff = sourcePaths.scriptStuff.concat(sourcePaths.typeScriptStuff);
var destScriptPath = 'wwwroot/js';
var destCssPath = 'wwwroot/css';

// Tasks
gulp.task('lint-scripts', function () {
    return gulp.src(sourcePaths.scriptStuff)
        .pipe(jshint())
        .pipe(jshint.reporter('default'));
});

// Cleanups
gulp.task('clean', function () {
    return del([destScriptPath + '/**/*', destCssPath + '/**/*']);
});

//push non-mofifying content 
gulp.task('move-content-to-wwwroot', function () {
    
    console.log(`moving scripts to ${destScriptPath}`);
    gulp.src(sourcePaths.scriptStuff)
        .pipe(debug())
        .pipe(gulp.dest(destScriptPath));
    
    console.log(`moving css to ${destCssPath}`);
    gulp.src(sourcePaths.cssFiles)
        .pipe(debug())
        .pipe(gulp.dest(destCssPath));
});


// Self hacking mermaid
//gulp.task('browserify-mermaid', ['lint-scripts'], function () {
gulp.task('browserify-mermaid', function () {
    return gulp.src('scripts/mermaid.nodejs')
        .pipe(browserify({
            insertGlobals: true
        }))
        .pipe(rename('mermaid.js'))
        .pipe(gulp.dest(destScriptPath));
});

gulp.task('refresh-bootstrap-sources',['refresh-bootstrap-sources-mixins'],function(){

    return gulp.src( ['bower_components/bootstrap/less/*.less'])
        .pipe(gulp.dest('less/reboot'));
});

gulp.task('refresh-bootstrap-sources-mixins', function(){

    return gulp.src( ['bower_components/bootstrap/less/mixins/*.less'])
        .pipe(gulp.dest('less/reboot/mixins'));
});

//just want a few bootstrap things, because it overrides stuff its not supposed to
gulp.task('repack-bootstrap', ['refresh-bootstrap-sources'], function() {
        
    return gulp.src('less/reboot/bootstrap.less')
        .pipe(gless())
        .pipe(gulp.dest('styles'));

});

