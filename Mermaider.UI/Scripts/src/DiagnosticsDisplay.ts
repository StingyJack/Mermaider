import $ from "../../bower_components/jquery-ts/index";

export class DiagnosticsDisplay {
    constructor(public containerId: string, public textAreaId: string) { }

    show(messages: any) {
        $(this.containerId).show();
        $(this.textAreaId).val(messages);//mermaidAPI.parseError
    }
    hide() {
        $(this.containerId).hide();
        $(this.textAreaId).val("");
    }
}