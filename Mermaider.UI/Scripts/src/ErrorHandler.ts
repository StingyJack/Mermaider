import $ from "../../bower_components/jquery-ts/index";
import { PageControls } from "./Constants";

export class ErrorHandler {
    constructor(public containerId: string, public textAreaId: string) {}

    clearErrors() {
        $(PageControls.errorsContainer).hide();
        $(PageControls.errorsList).val("");
    }


    handle(errorMessage: string, additionalData: any) {
        
    }

}
