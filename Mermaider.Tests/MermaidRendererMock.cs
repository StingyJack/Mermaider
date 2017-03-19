namespace Mermaider.Tests
{
    using Core;
    using Core.Abstractions;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal class MermaidRendererMock : Renderer
    {
        public MermaidRendererMock(string graphFilePath, string pathToNodeExe, string pathToMermaidJsFile) : base(graphFilePath, pathToNodeExe, pathToMermaidJsFile)
        {
            
        }
    }
}
