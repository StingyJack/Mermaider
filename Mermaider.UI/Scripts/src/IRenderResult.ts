export interface IRenderResult {
    isSuccessful: boolean,
    errors: string[],
    diagnostics: string[],
    localFileSystemImagePath: string,
    localUrlImagePath:string
}