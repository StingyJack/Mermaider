import $ from "../../bower_components/jquery-ts/index";

export class GraphText {
    constructor(public controlId: string) { }

    getText():string {
        return $(this.controlId).val();
    }

    setDefaultText() {
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
        
        $(this.controlId).val(defaultText);
    }
}