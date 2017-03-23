/// <reference path="../mermaid.d.ts" />
import $ from "../../bower_components/jquery-ts/index";
import { GraphText } from "./GraphText";
import { ErrorHandler } from "./ErrorHandler";
import { PageControls } from "./Constants";
import { RenderEngine } from "./RenderEngine";
import { DisplayElements } from "./DisplayElements";
import { DiagnosticsDisplay } from "./DiagnosticsDisplay";


declare var mermaid: IMermaid;


$(() => {
    App.startup();
});

class App {

    //static mermaid: IMermaid;
    static graphText: GraphText;
    static errorHandler: ErrorHandler;
    static renderEngine: RenderEngine;
    static diagnosticsDisplay: DiagnosticsDisplay;
    static displayElements: DisplayElements;

    static startup() {
        console.log("starting app...");

        console.log("setting default graph text");
        this.graphText = new GraphText(PageControls.dataEntryField);
        this.graphText.setDefaultText();

        console.log("creating error handler");
        this.errorHandler = new ErrorHandler(PageControls.errorsContainer, PageControls.errorsList);

        console.log("creating diagnostics display ");
        this.diagnosticsDisplay = new DiagnosticsDisplay(PageControls.diagnosticsContainer,
            PageControls.diagnosticsInfo);

        console.log("creating display elements...");
        this.displayElements = new DisplayElements(this.diagnosticsDisplay,
            PageControls.svgPreviewContainer,
            PageControls.openInNewWindowImageLinkContainer,
            PageControls.renderedImageContainer,
            mermaid);

        console.log("creating render engine");
        this.renderEngine = new RenderEngine(this.errorHandler, this.displayElements, this.diagnosticsDisplay);

        $(PageControls.startOver).click(() => location.reload());
        $(PageControls.renderImage).click(() => this.renderEngine.renderImage(this.graphText.getText()));
        $(PageControls.refreshPreview).click(() => this.renderEngine.attemptSvgPreview(this.graphText.getText()));

    }
}