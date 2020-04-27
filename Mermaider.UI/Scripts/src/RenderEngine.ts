import $ from "../../node_modules/jquery-ts/index";
import { PageControls } from "./Constants";
import { DisplayController } from "./DisplayController";
import { RenderResult } from "./RenderResult";
import { Logger } from "./Logger";

declare var mermaid: IMermaid;

export class RenderEngine {

    private log: Logger;

    constructor(private errorHandler: IAction<RenderResult>, private initializedLogger: Logger) {
        this.log = initializedLogger;
    }



    init() {

        const config = {
            startOnLoad: false
        };

        mermaid.initialize(config);
        mermaid.mermaidAPI.parseError = () => this.errorHandler;
    }

    canParse(graphText: string): boolean {
        this.log.setCheckpoint("preview");

        const parseable = mermaid.mermaidAPI.parse(graphText);
        return parseable;
    }

    renderPreview(graphText: string, responseHandler: IAction<string>) {
        
        //should be 10K attempts before repeat
        const needsUniqueId = `render${(Math.floor(Math.random() * 10000)).toString()}`;

        this.log.setCheckpoint(needsUniqueId);
        //var ele = $(PageControls.svgPreviewContainer).get(0);
        mermaid.mermaidAPI.render(needsUniqueId, graphText, (svgText: string) => responseHandler(svgText));

    }

    renderImage(graphText: string, responseHandler: IAction<RenderResult>) {
        this.log.setCheckpoint("imageRender");
        const data = { graphText: graphText };
        $.getJSON("/LiveEditor/RenderAsPng", data, (response: RenderResult) => responseHandler(response));
        //$.getJSON("/LiveEditor/RenderAsPng", data, (result: any): void => this.renderResponseHandler(result));

    }

}