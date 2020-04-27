import $ from "../../node_modules/jquery-ts/index";
import { RenderEngine } from "./RenderEngine";
import { PageControls } from "./Constants";
import { RenderResult } from "./RenderResult";
import { Logger } from "./Logger";

//import "hoverIntent";

export class DisplayController {

    private fadeDelay = 600;
    private opacityDim = 0.2;
    private zIndexActive = 10;
    private zIndexInactive = 1;
    private renderEngine: RenderEngine;
    private existingGraphIdent: string;
    private graphDisplay: PageControls.GraphDisplay;

    constructor(private mermaid: any, private logger: Logger) {

        console.log("creating render engine");
        this.graphDisplay = PageControls.GraphDisplay.None;
        this.renderEngine = new RenderEngine(() => this.errorHandler, logger);

        this.setDefaultText();
    }

    public wireEvents() {
        $(PageControls.startOver).click(() => location.reload());

        $(PageControls.renderImage).click(() => {
            this.dimElement(PageControls.controlContainer);
            event.stopImmediatePropagation();
            this.renderImage(this.getChartText());
        });

        $(PageControls.refreshPreview).click((event) => {
            this.dimElement(PageControls.controlContainer);
            event.stopImmediatePropagation();
            this.renderSvgPreview(this.getChartText());
        });
        $(PageControls.controlContainer).hover(() => {
            var t = setTimeout(() => {
                this.brightenElement(PageControls.controlContainer);
            }, 2000);
            $(this).data("timeout", t);
        },
            () => {
                clearTimeout($(this).data("timeout"));
            }
        );
        $(PageControls.controlContainer).click(() => this.brightenElement(PageControls.controlContainer));
    }

    private renderImage(graphText: string) {
        this.logger.setCheckpoint("image");
        this.setDisplayElementsBeforeRenderAction();

        const parseable = this.attemptParse(graphText);

        if (parseable === false) { return; }


        this.renderEngine.renderImage(graphText,
            (renderResult: RenderResult) => {

                if (renderResult.isSuccessful) {

                    this.existingGraphIdent = renderResult.graphIdent;
                    this.hideErrors(true);

                    const imagePath = renderResult.localUrlImagePath;
                    const link = `<a href="${imagePath}" class="btn-link" target="_blank">Open in new window</a>`;
                    const imageLink = `<img src="${imagePath}" class="mermaidRenderedImageContainerImage" alt="ShouldBeImage" />`;

                    this.showRenderedImage(link, imageLink);

                } else {

                    if (this.graphDisplay === PageControls.GraphDisplay.ImageRendered) {
                        
                        this.dimElement(PageControls.openInNewWindowImageLinkContainer);
                        this.dimElement(PageControls.renderedImageContainer);
                    }
                    this.showErrors(renderResult.errors.join("\n"), true);
                }

                this.showDiags(renderResult.diagnostics.join("\n"), true);
            });


    }

    private renderSvgPreview(graphText: string) {

        this.logger.setCheckpoint("preview");
        this.setDisplayElementsBeforeRenderAction();

        const parseable = this.attemptParse(graphText);

        if (parseable === false) { return; }

        this.renderEngine.renderPreview(graphText, (svgText) => this.handlePreviewResponse(svgText));

    }

    private setDisplayElementsBeforeRenderAction() {

        if (this.graphDisplay === PageControls.GraphDisplay.None) {
            this.hideElement(PageControls.svgPreviewContainer, true);
            this.hideElement(PageControls.openInNewWindowImageLinkContainer, true);
            this.hideElement(PageControls.renderedImageContainer);
        }
        else if (this.graphDisplay === PageControls.GraphDisplay.SvgPreview) {
            this.dimElement(PageControls.svgPreviewContainer);
            this.hideElement(PageControls.openInNewWindowImageLinkContainer, true);
            this.hideElement(PageControls.renderedImageContainer, true);
        }
        else if (this.graphDisplay === PageControls.GraphDisplay.ImageRendered) {
            this.hideElement(PageControls.svgPreviewContainer, true);
            this.dimElement(PageControls.openInNewWindowImageLinkContainer);
            this.dimElement(PageControls.renderedImageContainer);
        }
    }

    private attemptParse(graphText: string): boolean {
        const parseable = this.renderEngine.canParse(graphText);

        if (parseable === false) {
            this.showErrors("Failed to parse, check console.log", true);
            this.graphDisplay = PageControls.GraphDisplay.None;
            return false;
        }
        return true;
    }

    private handlePreviewResponse(svgText: string) {

        //no diags available for svg preview, console.log is the best I got for now.
        //const diags = this.logger.getLastOperationDiags();
        //this.showDiags(diags, true);

        this.graphDisplay = PageControls.GraphDisplay.SvgPreview;

        $(PageControls.svgPreviewContainer).html(svgText);
        this.existingGraphIdent = "preview";
        this.brightenElement(PageControls.svgPreviewContainer);

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

    private showElement(elementId: string): void {
        const opa = parseFloat($(elementId).css("opacity")).toFixed(1);
        if (opa !== "1.0") {
            $(elementId).fadeTo(this.fadeDelay, 1.0);
        } else {
            $(elementId).show();
        }
    }

    private brightenElement(elementId: string): void {
        $(elementId).fadeTo(this.fadeDelay, 1.0);
    }

    private dimElement(elementId: string) {
        const isVisible = $(elementId).is(":visible");
        if (isVisible) {
            $(elementId).fadeTo(this.fadeDelay, this.opacityDim);
        }
    }

    private hideElement(elementId: string, fade: boolean = false): void {
        if (fade) {
            $(elementId).fadeOut(this.fadeDelay);
        } else {
            $(elementId).hide();
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