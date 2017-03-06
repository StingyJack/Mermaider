"use strict";
var RenderResult = (function () {
    function RenderResult(hyperLink, result, errors, diagnostics) {
        this.hyperLink = hyperLink;
        this.result = result;
        this.errors = errors;
        this.diagnostics = diagnostics;
    }
    return RenderResult;
}());
exports.RenderResult = RenderResult;
//# sourceMappingURL=RenderResult.js.map