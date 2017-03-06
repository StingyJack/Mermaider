export class RenderResult {
    constructor(public hyperLink: string,
        public result: boolean,
        public errors: string[],
        public diagnostics: string[]
    ) {}


}