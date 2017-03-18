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
        public MermaidRendererMock(IFileUtils fileUtils, string graphFilePath, string imageFilePath) : base(graphFilePath)
        {
            _FileUtils = fileUtils;
        }
    }
}
