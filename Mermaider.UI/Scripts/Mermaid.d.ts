
interface IMermaidAPI {
    render: (id: string, txt: string, cb: Function, container?: Element) => void;
    parseError: (err: string, hash: string) => void;
    parse: (graphText: string) => boolean;
    
}

interface IMermaid {
    initialize: (options: any) => void;
    mermaidAPI: IMermaidAPI;
}

declare module "mermaid" {
    let MermaidAll: IMermaid;
    export = MermaidAll;
}

/*

exports.setMessage = function(txt){
    log.debug('Setting message to: '+txt);
    message = txt;
};

exports.getMessage = function(){
    return message;
};

exports.setInfo = function(inf){
    info = inf;
};

exports.getInfo = function(){
    return info;
};

exports.parseError = function(err,hash){
    global.mermaidAPI.parseError(err,hash);
};
*/