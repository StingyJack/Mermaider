"use strict";
var index_1 = require("../../bower_components/jquery-ts/index");
index_1.default(function () {
    App.startup();
});
var App = (function () {
    function App() {
    }
    App.startup = function () {
        this.graphText = new GraphText("#mermaidDataEntryField");
        this.graphText.setDefaultText();
    };
    return App;
}());
var GraphText = (function () {
    function GraphText(controlId) {
        this.controlId = controlId;
    }
    GraphText.prototype.setDefaultText = function () {
        var defaultText = "" +
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
        index_1.default(this.controlId).val(defaultText);
    };
    return GraphText;
}());
//# sourceMappingURL=app.js.map