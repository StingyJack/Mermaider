/// <reference path="../mermaid.d.ts" />
import $ from "../../node_modules/jquery-ts/index";
import { PageControls } from "./Constants";
import { RenderEngine } from "./RenderEngine";
import { DisplayController } from "./DisplayController";
import { Logger } from "./Logger";

declare var mermaid: IMermaid;


$(() => {
    App.startup();
});

class App {
    
    
    static logger: Logger;
    static displayElements: DisplayController;

    static startup() {
        this.logger = new Logger();
        this.logger.init();
        console.log("starting app...");
        
        console.log("creating display controller");
        this.displayElements = new DisplayController(mermaid, this.logger);
        
        console.log("wiring up events");
        this.displayElements.wireEvents();

        console.log("startup complete");
    }
}