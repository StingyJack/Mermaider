/// <binding AfterBuild='clean-target-outputs, move-content-to-wwwroot' Clean='clean-target-outputs' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

// Gulp Dependencies
var gulp = require("gulp");
var debug = require("gulp-debug");
var del = require("del");
var rename = require("gulp-rename");
var gless = require("gulp-less");
var cleanCss = require("gulp-clean-css");
var concatCss = require("gulp-concat-css");
var runSequence = require("run-sequence");
var browserify = require("gulp-browserify");
var uglify = require("gulp-uglify");
//var pocoGen = require('gulp-typescript-cs-poco');
var jshint = require("gulp-jshint");

// Common Setups - this should be copying the site.css too.
var sourcePaths = {
    typeScriptStuff: ["scripts/**/*.ts", "scripts/**/*.map"],
    scriptStuff: ["scripts/**/*.js", "bower_components/jquery/dist/*.*"],
    allScriptStuff: [],
    cssFiles: ["node_modules/mermaid/dist/*.css", "styles/*.*"]

};
sourcePaths.allScriptStuff = sourcePaths.scriptStuff.concat(sourcePaths.typeScriptStuff);

var destScriptPath = "wwwroot/js";
var destCssPath = "wwwroot/css";

// Cleaning Tasks
gulp.task("lint-scripts", function () {
    return gulp.src(sourcePaths.scriptStuff)
        .pipe(jshint())
        .pipe(jshint.reporter("default"));
});

gulp.task("clean-target-outputs", function () {
    return del([destScriptPath + "/**/*"
        , destCssPath + "/**/*"
        , "Less/reboot/mixin/*"
        , "!Less/reboot/mixin/"
        , "less/reboot/*"
        , "!less/reboot/bootstrap.less"]);
});

gulp.task("move-content-to-wwwroot", function () {

    console.log(`moving scripts to ${destScriptPath}`);
    gulp.src(sourcePaths.scriptStuff)
        .pipe(debug())
        .pipe(gulp.dest(destScriptPath));

    console.log(`moving css to ${destCssPath}`);
    gulp.src(sourcePaths.cssFiles)
        .pipe(debug())
        .pipe(concatCss("bundled.css"))
        .pipe(debug())
        .pipe(gulp.dest(destCssPath));
});

//these dosnt need to happen every compile
gulp.task("y-seldom-browserify-mermaid", ["lint-scripts"], function () {
    return gulp.src("scripts/mermaid.nodejs")
        .pipe(browserify({
            insertGlobals: true
        }))
        .pipe(rename("mermaid.js"))
        .pipe(gulp.dest("scripts"));
});

//just want a few bootstrap things, because it overrides the mermaid chart
// and there is no way to exclude specific elements. 
gulp.task("z-subtask-refresh-bootstrap-sources", function () {

    return gulp.src(["bower_components/bootstrap/less/*.less"])
        .pipe(gulp.dest("less/reboot"));
});

gulp.task("z-subtask-refresh-bootstrap-sources-mixins", function () {

    return gulp.src(["bower_components/bootstrap/less/mixins/*.less"])
        .pipe(gulp.dest("less/reboot/mixins"));
});

gulp.task("z-subtask-repack-bootstrap", function () {

    return gulp.src("less/reboot/bootstrap.less")
        .pipe(gless())
        .pipe(gulp.dest("styles"));

});

gulp.task("y-seldom-recompile-bootstrap", function () {
    runSequence("z-subtask-refresh-bootstrap-sources-mixins",
        "z-subtask-refresh-bootstrap-sources",
        "z-subtask-repack-bootstrap",
        "clean-target-outputs");


});

//gulp.task("generate-poco",
//    function () {
//        return gulp.src("mermaider.core/MermaidRenderResult.cs")
//            .pipe(debug())
//            .pipe(pocoGen({
//                ignoreMethods: true,
//                definitionFile: true
//            }))
//            .pipe(debug())
//            .pipe(gulp.dest("scripts/csPoco"));
//    });