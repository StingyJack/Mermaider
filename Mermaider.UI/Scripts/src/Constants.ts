export namespace PageControls {
    //something to create constants from the page control names would be nice - gulpfile?

    export const libraryTab = "#libraryTab";
    export const editorTab = "#editorTab";

    export const startOver = "#btnStartOver";
    export const renderImage = "#btnRenderImage";
    export const refreshPreview = "#btnRefreshPreview";
    export const controlContainer = "#controlContainer";
    export const dataEntryField = "#mermaidDataEntryField";
    export const svgPreviewContainer = "#svgPreviewContainer";
    export const openInNewWindowImageLinkContainer = "#openInNewWindowImageLinkContainer";
    export const renderedImageContainer = "#renderedImageContainer";

    export const errorsContainer = "#errorsContainer";
    export const errorMessage = "#errorMessage";

    export const diagnosticsContainer = "#diagnosticsContainer";
    export const diagnosticsInfo = "#diagnosticsInfo";

    export enum GraphDisplay {
        None,
        SvgPreview,
        ImageRendered
    };
}

