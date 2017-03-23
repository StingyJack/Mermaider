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
var webpack = require('webpack-stream');
var jshint = require("gulp-jshint");

// Common Setups - this should be copying the site.css too.
var sourcePaths = {
    typeScriptNonCompiledStuff: ["scripts/src/*.ts", "scripts/src/*.map"],
    typeScriptCompiledStuff: ["scripts/bin/scripts/src/*.*"],//, "scripts/src/*.map"],
    scriptStuff: ["scripts/*.js"],//, "bower_components/jquery/dist/*.*"],
    cssFiles: ["node_modules/mermaid/dist/*.css", "styles/*.*"]

};

var typeScriptCompilePath = "Scripts/bin/scripts/src";
var stagingScriptPath = "scripts";
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

gulp.task('repack-typescript',
    function () {
        return gulp.src(sourcePaths.typeScriptNonCompiledStuff)
            .pipe(debug())
            .pipe(browserify({
                insertGlobals: true
            }))
            .pipe(debug())
            .pipe(gulp.dest(typeScriptCompilePath));
    });

gulp.task("move-content-to-wwwroot", function () {


    var task1 = gulp.src(sourcePaths.scriptStuff)
        .pipe(debug())
        .pipe(gulp.dest(destScriptPath));

    var task2 = gulp.src(typeScriptCompilePath + "/*.js")
        .pipe(debug())
        .pipe(webpack({
                "output": {
                    "filename": "app.js"
                }
            }))
            .pipe(debug())
        .pipe(gulp.dest(destScriptPath));

    var task3 = gulp.src(sourcePaths.cssFiles)
        .pipe(debug())
        .pipe(concatCss("bundled.css"))
        .pipe(debug())
        .pipe(gulp.dest(destCssPath));

    return [task1, task2, task3];
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