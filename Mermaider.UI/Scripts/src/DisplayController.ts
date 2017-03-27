import $ from "../../bower_components/jquery-ts/index";
import { RenderEngine } from "./RenderEngine";
import { PageControls } from "./Constants";
import { RenderResult } from "./RenderResult";
import { Logger } from "./Logger";

export class DisplayController {

    private fadeDelay = 300;
    private opacityDim = 0.5;
    private renderEngine: RenderEngine;
    private existingGraphIdent: string;

    constructor(private mermaid: any, private logger: Logger) {

        console.log("creating render engine");
        this.renderEngine = new RenderEngine(() => this.errorHandler, logger);

        this.setDefaultText();
    }

    wireEvents() {
        $(PageControls.startOver).click(() => location.reload());
        $(PageControls.renderImage).click(() => this.renderImage(this.getChartText()));
        $(PageControls.refreshPreview).click(() => this.renderSvgPreview(this.getChartText()));
    }

    private renderImage(graphText: string) {
        this.logger.setCheckpoint("image");
        const parseable = this.renderEngine.canParse(graphText);
        if (parseable) {

            this.renderEngine.renderImage(graphText,
                (renderResult: RenderResult) => {
                    if (renderResult.isSuccessful) {
                        this.existingGraphIdent = renderResult.graphIdent;
                        this.hideErrors(false);
                        const imagePath = renderResult.localUrlImagePath;
                        const link = `<a href="${imagePath}" class="btn-link" target="_blank">Open in new window</a>`;
                        const imageLink = `<img src="${imagePath}" class="mermaidRenderedImageContainerImage" alt="ShouldBeImage" />`;

                        this.showRenderedImage(link, imageLink);
                        this.showDiags(renderResult.diagnostics.join("\n"), true);

                    } else {
                        this.dimGraphDisplayElements();
                        
                        this.showDiags(renderResult.diagnostics.join("\n"), true);
                        this.showErrors(renderResult.errors.join("\n"), true);
                    }
                });

        } else {
            this.showErrors("Failed to render, check console.log", true);
        }

    }

    private renderSvgPreview(graphText: string) {

        this.logger.setCheckpoint("preview");
        
        const isDisplayingPreview = (this.existingGraphIdent === "preview");
        const isDisplayingRenderedImage = (this.existingGraphIdent !== undefined);//starts with "render"

        //////////////////////////////////////////////////////
        //FIX The jerky animations to be consistent.
        //////////////////////////////////////////////////////



        //is this displaying a preview now?
        if (isDisplayingPreview) {
            //  yes - dim it and hide the rendered image display items
            this.dimElement(PageControls.svgPreviewContainer);
            $(PageControls.openInNewWindowImageLinkContainer).hide();
            $(PageControls.renderedImageContainer).hide();
        } else if (isDisplayingRenderedImage) {
            // no - but a rendered image is displayed, dim those controls
            this.dimElement(PageControls.openInNewWindowImageLinkContainer);
            this.dimElement(PageControls.renderedImageContainer);
        }
        else {
            //no - the handler will call show when appropriate.
            $(PageControls.svgPreviewContainer).hide();
        }

        const parseable = this.renderEngine.canParse(graphText);
        const diags = this.logger.getLastOperationDiags("preview");
        const logs = this.logger.getLastOperationLogLogs("preview");
        if (parseable === false) {

            this.showDiags(diags, true);
            this.showErrors("Failed to render, check console.log", true);

            if (this.existingGraphIdent != undefined) {
                $(PageControls.svgPreviewContainer).fadeTo(this.fadeDelay, 0.2);
            }
            return;
        }

        this.existingGraphIdent = "preview";
        this.renderEngine.renderPreview(graphText, (svgText) => this.handlePreviewResponse(svgText));

    }

    handlePreviewResponse(svgText: string) {

        $(PageControls.svgPreviewContainer).hide(this.fadeDelay);
        $(PageControls.svgPreviewContainer).html(svgText);
        if ($(PageControls.svgPreviewContainer).css("opacity") === "1.0") {
            $(PageControls.svgPreviewContainer).show();
        } else {
            $(PageControls.svgPreviewContainer).fadeIn(this.fadeDelay);
        }

    }


    private hideAllGraphDisplayElements(fadeAll: boolean) {

        if (fadeAll) {
            $(PageControls.svgPreviewContainer).fadeOut(this.fadeDelay);
            $(PageControls.openInNewWindowImageLinkContainer).fadeOut(this.fadeDelay);
            $(PageControls.renderedImageContainer).fadeOut(this.fadeDelay);
        } else {
            $(PageControls.svgPreviewContainer).hide();
            $(PageControls.openInNewWindowImageLinkContainer).hide();
            $(PageControls.renderedImageContainer).hide();
        }
    }

    private dimGraphDisplayElements() {

        $(PageControls.svgPreviewContainer).fadeOut(this.fadeDelay);
        $(PageControls.openInNewWindowImageLinkContainer).fadeOut(this.fadeDelay);
        $(PageControls.renderedImageContainer).fadeOut(this.fadeDelay);
    }

    private dimElement(elementId: string) {
        const isVisible = $(elementId).is(":visible");
        if (isVisible) {
            $(elementId).fadeTo(this.fadeDelay, this.opacityDim);
        }
    }

    private fadeElement(elementId: string) {
        const isVisible = $(elementId).is(":visible");
        if (isVisible) {
            $(elementId).fadeTo(this.fadeDelay, this.opacityDim);
        }
    }

    private showRenderedImage(hrefElement: string, imageElement: string) {
        $(PageControls.svgPreviewContainer).hide();
        $(PageControls.svgPreviewContainer).html("");

        $(PageControls.openInNewWindowImageLinkContainer).show();
        $(PageControls.openInNewWindowImageLinkContainer).html(hrefElement);

        //        $(this.renderedImageContainer).show();
        $(PageControls.renderedImageContainer).html(imageElement);
        $(PageControls.renderedImageContainer).fadeIn(this.fadeDelay);
    }

    private hideDiags(fadeOut: boolean) {
        if (fadeOut) {
            $(PageControls.diagnosticsContainer).fadeOut(this.fadeDelay);
        } else {
            $(PageControls.diagnosticsContainer).hide();
        }
    }

    private showDiags(messages: any, fadeIn: boolean) {
        $(PageControls.diagnosticsInfo).val(messages);
        if (fadeIn) {
            $(PageControls.diagnosticsContainer).fadeIn(this.fadeDelay);
        } else {
            $(PageControls.diagnosticsContainer).show();
        }
    }

    private hideErrors(fadeOut: boolean) {
        if (fadeOut) {
            $(PageControls.diagnosticsContainer).fadeOut(this.fadeDelay);
        } else {
            $(PageControls.diagnosticsContainer).hide();
        }
    }

    private showErrors(messages: any, fadeIn: boolean) {
        $(PageControls.errorMessage).val(messages); //mermaidAPI.parseError
        if (fadeIn) {
            $(PageControls.errorsContainer).fadeIn(this.fadeDelay);
        } else {
            $(PageControls.errorsContainer).show();
        }
    }

    private errorHandler(err: string, hash: any) {
        const result = new RenderResult();
        if (err === "0") {
            result.isSuccessful = true;
        } else {
            result.isSuccessful = false;
            result.errors.push(err);
        }

        return result;
    }

    private getChartText(): string {
        return $(PageControls.dataEntryField).val();
    }

    private setDefaultText() {
        const defaultText = "" +
            "graph TD  	 \n" +
            "    A-->B(\"<b>nice</b> <i>display</i> value\") 	 \n" +
            "    B-->C{\"with formatting?\"} 	 \n" +
            "    C-->|Yes|D1[\"html creole and others\"] 	 \n" +
            "    C-->|No|E2(\"or plain css styling\") 	 \n" +
            " 	 \n" +
            "    subgraph This 	 \n" +
            "     D1-->E1(\"...or some other stuff\") 	 \n" +
            "     E1-->F1((\" if you feel like it\")) 	 \n" +
            "    end 	 \n" +
            " 	 \n" +
            "    subgraph Or That 	 \n" +
            "     E2-->F2>\"and weird shapes\"] 	 \n" +
            "    end 	 \n" +
            " 	 \n" +
            "    F1-->Z(\"the end\") 	 \n" +
            "    F2-->Z  	 \n" +
            "  	 \n" +
            "classDef styled fill:#f9f,stroke:#333,stroke-width:4px;  	 \n" +
            "  	 \n" +
            "class F2 styled";

        $(PageControls.dataEntryField).val(defaultText);
    }

}