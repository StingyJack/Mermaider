export class RenderResult {
    // ReSharper disable WrongPublicModifierSpecification
    graphIdent: string;
    isSuccessful: boolean;
    errors: string[];
    diagnostics: string[];
    localFileSystemImagePath: string;
    localUrlImagePath: string;
    svgContent: string;
}
