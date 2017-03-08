namespace Mermaider.Tests
{
    using Core;
    using Core.Abstractions;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    internal class MermaidRendererMock : MermaidRenderer
    {
        public MermaidRendererMock(IFileUtils fileUtils, string graphFilePath, string imageFilePath) : base(graphFilePath, imageFilePath)
        {
            _FileUtils = fileUtils;
        }
    }
}
