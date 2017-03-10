declare module server {
	interface mermaidRenderResult {
		isSuccessful: boolean;
		svgContent: string;
		imagePath: string;
		errors: string[];
		diagnostics: string[];
	}
}
