"use strict";
/// <reference path="./mermaid.d.ts" />
var index_1 = require("../../bower_components/jquery-ts/index");
var GraphText_1 = require("./GraphText");
var ErrorHandler_1 = require("./ErrorHandler");
var Constants_1 = require("./Constants");
var RenderEngine_1 = require("./RenderEngine");
var DisplayElements_1 = require("./DisplayElements");
var DiagnosticsDisplay_1 = require("./DiagnosticsDisplay");
//import { IMermaidAPI } from ".,/Mermaid.js";
//import mer = require("../mermaid.js");
index_1.default(function () {
    App.startup();
});
var App = (function () {
    function App() {
    }
    App.startup = function () {
        var _this = this;
        console.log("starting app...");
        console.log("setting default graph text");
        this.graphText = new GraphText_1.GraphText(Constants_1.PageControls.dataEntryField);
        this.graphText.setDefaultText();
        console.log("creating error handler");
        this.errorHandler = new ErrorHandler_1.ErrorHandler(Constants_1.PageControls.errorsContainer, Constants_1.PageControls.errorsList);
        console.log("creating diagnostics display ");
        this.diagnosticsDisplay = new DiagnosticsDisplay_1.DiagnosticsDisplay(Constants_1.PageControls.diagnosticsContainer, Constants_1.PageControls.diagnosticsInfo);
        console.log("creating display elements...");
        this.displayElements = new DisplayElements_1.DisplayElements(this.diagnosticsDisplay, Constants_1.PageControls.svgPreviewContainer, Constants_1.PageControls.openInNewWindowImageLinkContainer, Constants_1.PageControls.renderedImageContainer, this.mermaid);
        console.log("creating render engine");
        this.renderEngine = new RenderEngine_1.RenderEngine(App.mermaid, this.errorHandler, this.displayElements, this.diagnosticsDisplay);
        index_1.default(Constants_1.PageControls.startOver).click(function () { return location.reload(); });
        index_1.default(Constants_1.PageControls.renderImage).click(function () { return _this.renderEngine.renderImage(_this.graphText.getText()); });
        index_1.default(Constants_1.PageControls.refreshPreview).click(function () { return _this.renderEngine.attemptSvgPreview(_this.graphText.getText()); });
    };
    return App;
}());
//# sourceMappingURL=App.js.map