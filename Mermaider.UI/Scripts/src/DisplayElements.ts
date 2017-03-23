import $ from "../../bower_components/jquery-ts/index";
import { DiagnosticsDisplay } from "./DiagnosticsDisplay";

export class DisplayElements {
    constructor(public diagnostics: DiagnosticsDisplay,
        public svgPreviewContainerId: string,
        public openInNewWindowImageLinkContainerId: string,
        public renderedImageContainerId: string,
        public mermaid:any) {
    }

    hideAll() {
        $(this.svgPreviewContainerId).hide();
        $(this.openInNewWindowImageLinkContainerId).hide();
        $(this.renderedImageContainerId).hide();
    }

    showSvgPreview(svgText: string) {

        $(this.svgPreviewContainerId).show();
        $(this.openInNewWindowImageLinkContainerId).hide();
        $(this.renderedImageContainerId).hide();
        $(this.svgPreviewContainerId).html(svgText);
        this.diagnostics.show(this.mermaid.mermaidAPI.parseError);
        //this.rmaidAPI logs to the console.log. Try to redirect it if possible, or just add another property with the string[]
    }

    showRenderedImage(hrefElement:string,imageElement: string) {
        $(this.svgPreviewContainerId).hide();
        $(this.openInNewWindowImageLinkContainerId).show();
        $(this.openInNewWindowImageLinkContainerId).html(hrefElement);
        $(this.renderedImageContainerId).show();
        $(this.renderedImageContainerId).html(imageElement);
    }
}