import $ from "../../bower_components/jquery-ts/index";
import { PageControls } from "./Constants";
import { ErrorHandler } from "./ErrorHandler";
import { DisplayElements } from "./DisplayElements";
import { DiagnosticsDisplay } from "./DiagnosticsDisplay";
import { IRenderResult } from "./IRenderResult";

declare var mermaid: IMermaid;

export class RenderEngine {
    constructor(public errorHandler: ErrorHandler,
        public displayElements: DisplayElements,
        public diagnostics: DiagnosticsDisplay) { }



    init() {
        const config = {
            startOnLoad: false
        };
        mermaid.initialize(config);

        mermaid.mermaidAPI.parseError = this.parseError;
    }

    attemptSvgPreview(graphText: string) {

        const parseable = mermaid.mermaidAPI.parse(graphText);
        if (parseable) {
            //should be 10K attempts before repeat
            const needsUniqueId = `render${(Math.floor(Math.random() * 10000)).toString()}`;

            mermaid.mermaidAPI.render(needsUniqueId, graphText, this.parseError);

        } else {
            this.errorHandler.handle("failed to render", "0"); //TODO: get console.log
        }
    }

    renderImage(graphText: string) {
        //TODO: FIll out
        var data = { graphText: graphText };
        $.getJSON("/LiveEditor/RenderAsPng", data, (result: any): void => this.handleRenderResponse(result));

    }

    handleRenderResponse(renderResult: IRenderResult) {

        if (renderResult.isSuccessful) {
            const imagePath = renderResult.localUrlImagePath;
            const link = `<a href="${imagePath}" class="btn-link" target="_blank">Open in new window</a>`;
            const imageLink = `<img src="${imagePath}" class="mermaidRenderedImageContainerImage" alt="ShouldBeImage" />`;

            this.displayElements.showRenderedImage(link, imageLink);
          
            this.diagnostics.show(renderResult.diagnostics.join('\n'));

        } else {
            this.displayElements.hideAll();
            this.diagnostics.show(renderResult.diagnostics.join('\n'));
            this.errorHandler.handle(renderResult.errors.join('\n'), "0");
        }
    }

    parseError(err: string, hash: any) {
        if (err === "0") {
            this.errorHandler.clearErrors();

        }
        else {
            this.errorHandler.handle(err, hash);
            this.displayElements.hideAll();
        }
    }


}